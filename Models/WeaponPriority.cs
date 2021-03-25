namespace ImpactApi.Models
{
    public class WeaponPriority
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string CharacterId { get; set; }
        public string CharacterRole { get; set; }
        public string WeaponId { get; set; }
        public Weapon Weapon { get; set; }
    }
}
