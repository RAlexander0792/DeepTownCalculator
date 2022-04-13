namespace DeepTownCalculator.Domain.Entities
{
    public class AreaEntity
    {
        public int AreaID { get; set; }
        public int AreaNumber { get; set; }
        public bool Oil { get; set; }
        public IEnumerable<(int, ItemEntity)> Resources { get; set; }

        public AreaEntity(IEnumerable<(int, ItemEntity)> resources)
        {
            Resources = resources;
        }
    }
}
