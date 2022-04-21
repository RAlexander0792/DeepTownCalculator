namespace DeepTownCalculator.Domain.Entities
{
    public class AreaEntity
    {
        public int Id { get; set; }
        public int AreaNumber { get; set; }
        public bool Oil { get; set; }
        public IEnumerable<(int, ItemEntity)> Resources { get; set; }

        public AreaEntity()
        {
            Resources = new List<(int, ItemEntity)>();
        }
        public AreaEntity(IEnumerable<(int, ItemEntity)> resources)
        {
            Resources = resources;
        }
    }
}
