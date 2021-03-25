using System.Collections.Generic;

namespace ImpactApi.Models
{
    public class Weapon
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAtk { get; set; }
        public string SubStatType { get; set; }
        public float SubStat { get; set; }
        public string AbilityName { get; set; }
        public string AbilityDescription { get; set; }
        public string Description { get; set; }
        public string Lore { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public List<WeaponStat> Stats { get; set; }
    }
}
