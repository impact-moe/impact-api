using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using ImpactApi.Entities;
using ImpactApi.Models;
using ImpactApi.Settings;

namespace ImpactApi.Services
{
    public class JwtService
    {
		private readonly JwtSettings _jwtSettings;
		private readonly UserManager<User> _userManager;

        public JwtService(JwtSettings jwtSettings, UserManager<User> userManager) {
            this._userManager = userManager;
			this._jwtSettings = jwtSettings;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(SocialAuth socialAuth)
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(socialAuth.IdToken);

            return payload;
        }

		public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using(RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    IpAddress = ipAddress
                };
            }
        }

        public string GenerateToken(User user)
		{
			SigningCredentials signingCredentials = GetSigningCredentials();
			List<Claim> claims = GetClaims(user);
			JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);

			return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		}

        private SigningCredentials GetSigningCredentials()
		{
			byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
			SymmetricSecurityKey secret = new SymmetricSecurityKey(key);

			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}

        private List<Claim> GetClaims(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, user.Email)
			};

			return claims;
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,

				ValidIssuer = _jwtSettings.ValidIssuer,
				ValidAudience = _jwtSettings.ValidAudience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey))
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;
		}

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
		{
			JwtSecurityToken tokenOptions = new JwtSecurityToken(
				issuer: _jwtSettings.ValidIssuer,
				audience: _jwtSettings.ValidAudience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
				signingCredentials: signingCredentials);

			return tokenOptions;
		}
    }
}
