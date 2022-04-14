using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepTownCalculator.Data.File.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        IEnumerable<AreaEntity> IAreaRepository.Get(int areaNumber)
        {
            throw new NotImplementedException();
        }

        IEnumerable<AreaEntity> IAreaRepository.Get(IEnumerable<int> areaNumbers)
        {
            throw new NotImplementedException();
        }

        IEnumerable<AreaEntity> IAreaRepository.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
