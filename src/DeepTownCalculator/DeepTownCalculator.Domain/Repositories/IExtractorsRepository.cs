using DeepTownCalculator.Domain.Entities;

namespace DeepTownCalculator.Domain.Repositories
{
    public interface IExtractorsRepository
    {
        IEnumerable<ExtractorEntity> GetAll();
    }
}
