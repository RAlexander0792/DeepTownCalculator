using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepTownCalculator.UnitTests.Mockers
{
    public class AreaRepositoryMocker
    {
        Mock<IAreaRepository> _areaRepository;
        List<ItemEntity> Items;
        List<AreaEntity> Areas;
        public AreaRepositoryMocker()
        {
            _areaRepository = new();
            Items = new();
            Areas = new();

        }
        public AreaRepositoryMocker WithItems(IEnumerable<ItemEntity> items)
        {
            this.Items.AddRange(items);
            return this;
        }
        public AreaRepositoryMocker WithArea(int areaNumber, IEnumerable<(int, Guid)> perc_items, bool oil = false)
        {
            Areas.Add(new AreaEntity
            {
                Id = Areas.Count + 1,
                AreaNumber = areaNumber,
                Oil = oil,
                Resources = perc_items.Select(x => (x.Item1, Items.First(c => c.Id == x.Item2)))
            });

            return this;
        }
        public AreaRepositoryMocker SetupGet(int areaNumber)
        {
            _areaRepository.Setup(x => x.Get(areaNumber)).Returns(Areas.Where(x => x.AreaNumber == areaNumber));
            return this;
        }
        public AreaRepositoryMocker SetupGet(IEnumerable<int> areaNumber)
        {
            _areaRepository.Setup(x => x.Get(areaNumber)).Returns(Areas.Where(x => areaNumber.Contains(x.AreaNumber)));
            return this;
        }


        public IAreaRepository Build()
        {
            return _areaRepository.Object;
        }
    }
}
