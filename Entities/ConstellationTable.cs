namespace ImpactApi.Entities
{
    public partial class Constellation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public sbyte Order { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CharacterId { get; set; }
    }
}
