using System.Collections.Generic;

namespace ImpactApi.Models
{
    public class Role
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public List<WeaponPriority> Weapons { get; set; }
        public List<ArtifactPriority> Artifacts { get; set; }
        public List<MainStatPriority> MainStats{ get; set; }
        public List<SubStatPriority> SubStats { get; set; }
        public Role()
        {
            Weapons = new List<WeaponPriority>();
            Artifacts = new List<ArtifactPriority>();
            MainStats = new List<MainStatPriority>();
            SubStats = new List<SubStatPriority>();
        }
    }
}
