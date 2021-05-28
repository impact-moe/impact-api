using System.Collections.Generic;

namespace ImpactApi.Entities
{
    public partial class Character
    {
        public Character()
        {
            ArtifactPriorities = new HashSet<ArtifactPriority>();
            MainStatPriorities = new HashSet<MainStatPriority>();
            Roles = new HashSet<Role>();
            SubStatPriorities = new HashSet<SubStatPriority>();
            WeaponPriorities = new HashSet<WeaponPriority>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public sbyte Rarity { get; set; }
        public string Weapon { get; set; }
        public string Element { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string Quote { get; set; }
        public string SquareCard { get; set; }
        public string Title { get; set; }
        public string Faction { get; set; }
        public string Birthday { get; set; }
        public string Constellation { get; set; }
        public string ChineseVa { get; set; }
        public string JapaneseVa { get; set; }
        public string EnglishVa { get; set; }
        public string KoreanVa { get; set; }

        public virtual ICollection<ArtifactPriority> ArtifactPriorities { get; set; }
        public virtual ICollection<MainStatPriority> MainStatPriorities { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<SubStatPriority> SubStatPriorities { get; set; }
        public virtual ICollection<WeaponPriority> WeaponPriorities { get; set; }
    }
}
