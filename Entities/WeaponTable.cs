using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImpactApi.Entities
{
    public partial class Weapon
    {
        public Weapon()
        {
            WeaponPriorities = new HashSet<WeaponPriority>();
            WeaponStats = new HashSet<WeaponStat>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAtk { get; set; }
        public string SubStatType { get; set; }
        public decimal SubStat { get; set; }
        public string AbilityName { get; set; }
        public string AbilityDescription { get; set; }
        public string Description { get; set; }
        public string Lore { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }

        public virtual ICollection<WeaponStat> WeaponStats { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<WeaponPriority> WeaponPriorities { get; set; }
    }
}
