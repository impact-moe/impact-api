using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImpactApi.Entities
{
    public partial class Role
    {
        public string Id { get; set; }
        public string CharacterId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<ArtifactPriority> ArtifactPriorities { get; set; } = new List<ArtifactPriority>();
        public virtual ICollection<MainStatPriority> MainStatPriorities { get; set; } = new List<MainStatPriority>();
        public virtual ICollection<SubStatPriority> SubStatPriorities { get; set; } = new List<SubStatPriority>();
        public virtual ICollection<WeaponPriority> WeaponPriorities { get; set; } = new List<WeaponPriority>();

        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}
