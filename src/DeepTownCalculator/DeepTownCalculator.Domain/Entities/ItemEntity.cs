namespace DeepTownCalculator.Domain.Entities
{
    public class ItemEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
    }
}
