﻿using Newtonsoft.Json;

namespace ImpactApi.Entities
{
    public partial class CharacterOverview
    {
        public string CharacterId { get; set; }
        public string AbilityTips { get; set; }
        public string RecommendedRole { get; set; }
        
        [JsonIgnore]
        public Character Character { get; set; }
    }
}