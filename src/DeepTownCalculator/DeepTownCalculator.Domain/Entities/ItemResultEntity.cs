namespace DeepTownCalculator.Domain.Entities
{
    public class ItemResultEntity
    {
        public Guid ItemId { get; set; }
        public decimal RPM { get; set; }
        public decimal DepletedInMins { get; set; }
        public decimal DepletedInHours { get; set; }
        public decimal Reserve { get; set; }
        public decimal Produced { get; set; }

    }
}
