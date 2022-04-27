using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Enums;
using DeepTownCalculator.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;

namespace DeepTownCalculator.UnitTests.Mockers
{
    public class ExtractorsRepositoryMocker
    {
        Mock<IExtractorsRepository> _extractorsRepository;
        List<ExtractorEntity> Extractors;
        public ExtractorsRepositoryMocker()
        {
            _extractorsRepository = new();
            Extractors = new();

        }
        public ExtractorsRepositoryMocker WithMine(int level, decimal RPM)
        {
            Extractors.Add(new ExtractorEntity
            {
                Id = Guid.NewGuid(),
                ExtractorType = ExtractorType.Mine,
                Level = level,
                RPM = RPM
            });

            return this;
        }

        public IExtractorsRepository Build()
        {
            _extractorsRepository.Setup(x => x.GetAll()).Returns(Extractors);
            return _extractorsRepository.Object;
        }
    }
}
