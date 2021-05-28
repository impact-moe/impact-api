using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ImpactApi.Entities {
    [Table("AspNetUserRefreshTokens")]
    public class RefreshToken {

        [Key]
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string IpAddress { get; set; }
    }
}