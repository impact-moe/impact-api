using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ImpactApi.Entities {
    public class User : IdentityUser {
        public string DisplayImage { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}