using CodenameGenerator;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ImpactApi.Entities;
using ImpactApi.Models;
using ImpactApi.Services;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly List<string> _characterNames = new List<string> {
            "Albedo",
            "Amber",
            "Ayaka",
            "Barbara",
            "Beidou",
            "Bennett",
            "Chongyun",
            "Dainsleif",
            "Diluc",
            "Diona",
            "Eula",
            "Fischl",
            "Ganyu",
            "HuTao",
            "Jean",
            "Kaeya",
            "Kazuha",
            "Keqing",
            "Klee",
            "Lisa",
            "Mimi",
            "Mona",
            "Ningguang",
            "Noelle",
            "Paimon",
            "Qiqi",
            "Razor",
            "Rosaria",
            "Sayu",
            "Shenli",
            "Sucrose",
            "Tartaglia",
            "Venti",
            "Xiangling",
            "Xiao",
            "Xingqiu",
            "Xinyan",
            "Yanfei",
            "Yaoyao",
            "Yunjin",
            "Zhongli"
        };
        private readonly Generator _generator;
        private readonly JwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersController(JwtService jwtService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._jwtService = jwtService;
            this._userManager = userManager;
            this._signInManager = signInManager;

            _generator = new Generator();
            _generator.SetParts(WordBank.Adjectives);
            _generator.Casing = Casing.PascalCase;
        }

        [Authorize]
        [HttpPost("updateuser")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User newUserInfo)
        {
            User user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if (user == null)
                return BadRequest("Invalid Account");

            user.UserName = newUserInfo.UserName;
            user.DisplayImage = newUserInfo.DisplayImage;

            IdentityResult identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors);

            return Ok(user);
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<User>> GetUser(string userName)
        {
            User user = await _userManager.Users.FirstAsync(o => o.UserName == userName);

            if (user == null)
                return NoContent();

            return Ok(user);
        }

        [HttpPut("revoketoken")]
        public async Task<ActionResult> RevokeToken(RefreshToken refreshToken)
        {
            User user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            if (user == null)
                return BadRequest("Invalid Account");

            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            if (user.RefreshTokens.RemoveAll(o => o.Token.Equals(refreshToken.Token) && o.IpAddress.Equals(ipAddress)) == 0)
                return BadRequest("Invalid Refresh Token");

            return Ok();
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshToken refreshToken)
        {
            string authHeader = Request.Headers["Authorization"];

            if (authHeader.Equals(String.Empty))
                return BadRequest("Invalid Expired Token");

            AuthenticationHeaderValue authHeaderValue;
            if (!AuthenticationHeaderValue.TryParse(authHeader, out authHeaderValue))
                return BadRequest("Invalid Bearer Token Format");

            string expiredToken = authHeaderValue.Parameter;

            if (expiredToken == null)
                return BadRequest("Invalid Bearer Token Format");

            ClaimsPrincipal User = _jwtService.GetPrincipalFromExpiredToken(expiredToken);

            if (User == null)
                return BadRequest("Invalid Expired Token");

            User user = await _userManager.Users
                .Include(o => o.RefreshTokens)
                .SingleAsync(o => o.Email == User.FindFirstValue(ClaimTypes.Email));

            if (user == null)
                return BadRequest("Invalid Account");

            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            if (refreshToken.Token == null)
                return BadRequest("Invalid Refresh Token");

            if (user.RefreshTokens.RemoveAll(o => o.Token.Equals(refreshToken.Token) && o.IpAddress.Equals(ipAddress)) == 0)
                return BadRequest("Invalid Refresh Token");

            RefreshToken newRefreshToken = _jwtService.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors);

            string newToken = _jwtService.GenerateToken(user);

            AuthResponse authResponse = new AuthResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                DisplayImage = user.DisplayImage,
                UserName = user.UserName
            };

            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] SocialAuth socialAuth)
        {
            string providerKey;
            string userEmail;
            string userImage;

            if (socialAuth.Provider.Equals("GOOGLE"))
            {
                GoogleJsonWebSignature.Payload payload = await _jwtService.VerifyGoogleToken(socialAuth);

                if (payload == null)
                    return BadRequest("Invalid Google Authentication Token");

                providerKey = payload.Subject;
                userEmail = payload.Email;
                userImage = payload.Picture;
            }
            else
                return BadRequest("Invalid Login Provider");

            UserLoginInfo info = new UserLoginInfo(socialAuth.Provider, providerKey, socialAuth.Provider);
            User user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            IdentityResult identityResult;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    user = new User
                    {
                        Email = userEmail,
                        UserName = _generator.Generate() + _characterNames[(new Random()).Next(_characterNames.Count)],
                        DisplayImage = userImage
                    };

                    identityResult = await _userManager.CreateAsync(user);
                    if (!identityResult.Succeeded)
                        return BadRequest(identityResult.Errors);

                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                return BadRequest("Invalid Authentication");

            string token = _jwtService.GenerateToken(user);

            string ipAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            RefreshToken newRefreshToken = _jwtService.GenerateRefreshToken(ipAddress);

            user = await _userManager.Users
            .Include(o => o.RefreshTokens)
            .FirstAsync(o => o.Id.Equals(user.Id));

            user.RefreshTokens.Add(newRefreshToken);

            identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors);

            AuthResponse authResponse = new AuthResponse
            {
                Token = token,
                RefreshToken = newRefreshToken,
                DisplayImage = userImage,
                UserName = user.UserName
            };

            return Ok(authResponse);
        }
    }
}
