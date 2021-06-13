using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class ArtifactSet
    {
        public ArtifactSet()
        {
            ArtifactPriorities = new HashSet<ArtifactPriority>();
            Artifacts = new HashSet<Artifact>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int MaxRarity { get; set; }
        public string TwoPieceBonus { get; set; }
        public string FourPieceBonus { get; set; }

        [JsonIgnore]
        public virtual ICollection<ArtifactPriority> ArtifactPriorities { get; set; }

        [JsonIgnore]
        public virtual ICollection<Artifact> Artifacts { get; set; }
    }
}
