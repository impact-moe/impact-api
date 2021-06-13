namespace ImpactApi.Entities
{
    public partial class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Effect { get; set; }
        public string Type { get; set; }
        public string IngredientOne { get; set; }
        public string IngredientTwo { get; set; }
        public string IngredientThree { get; set; }
        public string IngredientFour { get; set; }
        public string Image { get; set; }
        public string CharacterName { get; set; }
    }
}
