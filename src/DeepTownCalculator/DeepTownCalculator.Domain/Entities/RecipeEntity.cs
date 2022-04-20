namespace DeepTownCalculator.Domain.Entities
{
    public class RecipeEntity
    {
        public int RecipeId { get; set; }
        public TimeSpan Time { get; set; }
        public IEnumerable<(int, ItemEntity)> Ingredients { get; set; }
        public IEnumerable<(int, ItemEntity)> Products { get; set; }

        public RecipeEntity()
        {
            Ingredients = new List<(int, ItemEntity)>();
            Products = new List<(int, ItemEntity)>();
        }
    }
}