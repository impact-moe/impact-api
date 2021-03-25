using System.Data.Common;

namespace ImpactApi.Models
{
    public class ArtifactSet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MaxRarity { get; set; }
        public string TwoPieceBonus { get; set; }
        public string FourPieceBonus { get; set; }
    }
}
