using DeepTownCalculator.Domain.Entities;

namespace DeepTownCalculator.Domain.Repositories
{
    public interface IAreaRepository
    {
        IEnumerable<AreaEntity> Get(int areaNumber);
        IEnumerable<AreaEntity> Get(IEnumerable<int> areaNumbers);
        IEnumerable<AreaEntity> GetAll();
    }
}
