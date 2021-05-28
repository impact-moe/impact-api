using ImpactApi.Entities;

namespace ImpactApi.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string UserName { get; set; }
        public string DisplayImage { get; set; }
    }
}