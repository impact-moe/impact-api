﻿using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class SubStatPriority
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string Type { get; set; }
        public string CharacterId { get; set; }
        public string CharacterRole { get; set; }
        public string RoleId { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }
        
        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}