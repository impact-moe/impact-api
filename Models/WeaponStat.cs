namespace ImpactApi.Models
{
    public class WeaponStat
    {
        public int Id { get; set; }
        public string WeaponId { get; set; }
        public string Level { get; set; }
        public int BaseAtk { get; set; }
        public float SubStat { get; set; }
    }
}
