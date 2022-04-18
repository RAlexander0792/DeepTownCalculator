using DeepTownCalculator.Application;
using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Enums;
using DeepTownCalculator.Domain.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepTownCalculator.UnitTests.Services
{
    public class CalculatorServiceTest
    {
        [Test]
        public void CalculateRPM_Excavators_OnlyMines_Ok()
        {
            #region Arrange
            var Item1Id = Guid.NewGuid();
            var Item2Id = Guid.NewGuid();
            var Item3Id = Guid.NewGuid();

            List<AreaEntity> areaEntities = new List<AreaEntity>() {
                new AreaEntity { 
                        AreaID = 1,
                        AreaNumber = 1,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (50, new ItemEntity { Id = Item1Id, Name = "", Price = 0 }),
                                    (25, new ItemEntity { Id = Item2Id, Name = "", Price = 0 }),
                                    (25, new ItemEntity { Id = Item3Id, Name = "", Price = 0 })
                }},
            };
            List<int> areas = new() { 1 };
            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock.Setup(x => x.Get(areas)).Returns(areaEntities);

            var extractorsEntities = new List<ExtractorEntity>()
            {
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 1, RPM = 2 }
            };

            var extractorsRepositoryMock = new Mock<IExtractorsRepository>();
            extractorsRepositoryMock.Setup(x => x.GetAll()).Returns(extractorsEntities);


            var service = new CalculatorService(areaRepositoryMock.Object, extractorsRepositoryMock.Object);
            
            #endregion Arrange
            #region Act
            var result = service.CalculateRPM(
                new CalcRequest
                (
                    new List<Extractor> { new Extractor(1, 1, Guid.Empty, ExtractorType.Mine)},
                    new List<Crafter>(),
                    new List<InventoryItem>()
                ));
            #endregion Act
            #region Assert
            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id), "Item 3 not included in result");

            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id && x.RPM == 1.0m), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id && x.RPM == 0.5m), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id && x.RPM == 0.5m), "Item 3 doesn't have correct RPM");

            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id), "Item 1 doesn't have correct name");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id), "Item 2 doesn't have correct name");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id), "Item 3 doesn't have correct name");
            #endregion Assert


        }
    }
}
