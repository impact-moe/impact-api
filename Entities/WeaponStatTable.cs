using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class WeaponStat
    {
        public int Id { get; set; }
        public string WeaponId { get; set; }
        public string Level { get; set; }
        public int BaseAtk { get; set; }
        public decimal SubStat { get; set; }

        [JsonIgnore]
        public virtual Weapon Weapon { get; set; }
    }
}
