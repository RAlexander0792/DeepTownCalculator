using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Repositories;

namespace DeepTownCalculator.Data.File.Repositories
{
    public class ExtractorsRepository : IExtractorsRepository
    {
        IEnumerable<ExtractorEntity> IExtractorsRepository.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
