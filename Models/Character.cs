using System.Collections.Generic;

namespace ImpactApi.Models
{
    public class Character
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public string Rarity { get; set; }
        public string Weapon { get; set; }
        public string Element { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string Faction { get; set; }
        public string Image { get; set; }
        public string Quote { get; set; }
        public string Icon { get; set; }
        public string SquareCard { get; set; }
        public string Title { get; set; }
        public string Birthday { get; set; }
        public string Constellation { get; set; }
        public string ChineseVA { get; set; }
        public string JapaneseVA { get; set; }
        public string EnglishVA { get; set; }
        public string KoreanVA { get; set; }
        public List<Talent> Talents { get; set; }
        public List<Constellation> Constellations { get; set; }
        public List<Role> Roles { get; set; }
        public CharacterOverview CharacterOverview { get; set; }
    }
}
