using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class WeaponPriority
    {
        public int Id { get; set; }
        public string CharacterId { get; set; }
        public string CharacterRole { get; set; }
        public int Rank { get; set; }
        public string WeaponId { get; set; }
        public string RoleId { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }
        
        [JsonIgnore]
        public virtual Character Character { get; set; }
        public virtual Weapon Weapon { get; set; }
    }
}
