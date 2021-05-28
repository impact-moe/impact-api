using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class ArtifactPriority
    {
        public int Id { get; set; }
        public string CharacterId { get; set; }
        public string CharacterRole { get; set; }
        public int Rank { get; set; }
        public string ArtifactSetId { get; set; }

        public virtual ArtifactSet ArtifactSet { get; set; }
        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}
