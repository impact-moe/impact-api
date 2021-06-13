using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class Constellation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public sbyte Order { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public string CharacterId { get; set; }

        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}
