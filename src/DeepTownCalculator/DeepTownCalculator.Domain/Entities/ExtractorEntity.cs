using DeepTownCalculator.Domain.Enums;

namespace DeepTownCalculator.Domain.Entities
{
    public class ExtractorEntity
    {
        public Guid Id { get; set; }
        public ExtractorType ExtractorType { get; set; }
        public int Level { get; set; }
        public decimal RPM { get; set; }
    }
}
