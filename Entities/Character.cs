using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class Character
    {
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
        public string ChineseVA { get; set; }
        public string JapaneseVA { get; set; }
        public string EnglishVA { get; set; }
        public string KoreanVA { get; set; }

        public virtual ICollection<Talent> Talents { get; set; } = new List<Talent>();
        public virtual ICollection<Constellation> Constellations { get; set; } = new List<Constellation>();
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual CharacterOverview CharacterOverview { get; set; }

        [JsonIgnore]
        public virtual ICollection<ArtifactPriority> ArtifactPriorities { get; set; } = new List<ArtifactPriority>();
        
        [JsonIgnore]
        public virtual ICollection<MainStatPriority> MainStatPriorities { get; set; } = new List<MainStatPriority>();

        [JsonIgnore]
        public virtual ICollection<SubStatPriority> SubStatPriorities { get; set; } = new List<SubStatPriority>();

        [JsonIgnore]
        public virtual ICollection<WeaponPriority> WeaponPriorities { get; set; } = new List<WeaponPriority>();
    }
}
