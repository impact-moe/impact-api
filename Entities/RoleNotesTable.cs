using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string CharacterId { get; set; }
        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}
