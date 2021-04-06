namespace ImpactApi.Models
{
    public class Artifact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rarity { get; set; }
        public string Description { get; set; }
        public string Lore { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public ArtifactSet ArtifactSet { get; set; }
    }
}
