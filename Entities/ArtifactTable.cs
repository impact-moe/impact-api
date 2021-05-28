namespace ImpactApi.Entities
{
    public partial class Artifact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public sbyte Rarity { get; set; }
        public string Description { get; set; }
        public string Lore { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string ArtifactSetId { get; set; }

        public virtual ArtifactSet ArtifactSet { get; set; }
    }
}
