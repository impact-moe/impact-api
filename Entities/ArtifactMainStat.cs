namespace ImpactApi.Entities
{
    public partial class ArtifactMainStat
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string Stat { get; set; }
        public int? Tier { get; set; }
    }
}
