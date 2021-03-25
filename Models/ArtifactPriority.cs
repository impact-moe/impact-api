namespace ImpactApi.Models
{
    public class ArtifactPriority
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string CharacterId { get; set; }
        public string CharacterRole { get; set; }
        public string ArtifactSetId { get; set; }
        public ArtifactSet ArtifactSet { get; set; }
    }
}
