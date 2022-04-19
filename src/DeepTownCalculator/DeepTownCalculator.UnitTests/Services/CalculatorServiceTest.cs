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
        public void CalculateRPM_Excavators_Only1Mine_Ok()
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
                    new List<InventoryItem>(),
                    new List<TechLabBoost>()
                ));
            #endregion Act
            #region Assert
            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id), "Item 3 not included in result");

            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id && x.RPM == 1.0m), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id && x.RPM == 0.5m), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id && x.RPM == 0.5m), "Item 3 doesn't have correct RPM");

            #endregion Assert
        }
        [Test]
        public void CalculateRPM_Excavators_Only3Mines_Ok()
        {
            #region Arrange
            var Item1Id = Guid.NewGuid();
            var Item2Id = Guid.NewGuid();
            var Item3Id = Guid.NewGuid();
            var Item4Id = Guid.NewGuid();
            var Item5Id = Guid.NewGuid();

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
                new AreaEntity {
                        AreaID = 2,
                        AreaNumber = 2,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (100, new ItemEntity { Id = Item1Id, Name = "", Price = 0 }),
                }},
                new AreaEntity {
                        AreaID = 3,
                        AreaNumber = 3,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (33, new ItemEntity { Id = Item3Id, Name = "", Price = 0 }),
                                    (33, new ItemEntity { Id = Item4Id, Name = "", Price = 0 }),
                                    (33, new ItemEntity { Id = Item5Id, Name = "", Price = 0 })
                }},
            };
            List<int> areas = new() { 1, 2, 3 };
            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock.Setup(x => x.Get(areas)).Returns(areaEntities);

            var extractorsEntities = new List<ExtractorEntity>()
            {
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 1, RPM = 2 },
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 2, RPM = 4 },
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 3, RPM = 6 }
            };

            var extractorsRepositoryMock = new Mock<IExtractorsRepository>();
            extractorsRepositoryMock.Setup(x => x.GetAll()).Returns(extractorsEntities);


            var service = new CalculatorService(areaRepositoryMock.Object, extractorsRepositoryMock.Object);

            #endregion Arrange
            #region Act
            var result = service.CalculateRPM(
                new CalcRequest
                (
                    new List<Extractor> { 
                        new Extractor(1, 1, Guid.Empty, ExtractorType.Mine),
                        new Extractor(2, 2, Guid.Empty, ExtractorType.Mine),
                        new Extractor(3, 1, Guid.Empty, ExtractorType.Mine)
                    },
                    new List<Crafter>(),
                    new List<InventoryItem>(),
                    new List<TechLabBoost>()
                ));
            #endregion Act
            #region Assert
            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id), "Item 3 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item4Id), "Item 4 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item5Id), "Item 5 not included in result");

            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id && x.RPM == 1.0m + 4.0m), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id && x.RPM == 0.5m), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id && x.RPM == 0.5m + 0.66m), "Item 3 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item4Id && x.RPM == 0.66m), "Item 4 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item5Id && x.RPM == 0.66m), "Item 5 doesn't have correct RPM");
            #endregion Assert
        }

        [Test]
        public void CalculateRPM_Excavators_3Mines_ExtractorBoostOk()
        {
            #region Arrange
            var Item1Id = Guid.NewGuid();
            var Item2Id = Guid.NewGuid();
            var Item3Id = Guid.NewGuid();
            var Item4Id = Guid.NewGuid();
            var Item5Id = Guid.NewGuid();

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
                new AreaEntity {
                        AreaID = 2,
                        AreaNumber = 2,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (100, new ItemEntity { Id = Item1Id, Name = "", Price = 0 }),
                }},
                new AreaEntity {
                        AreaID = 3,
                        AreaNumber = 3,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (33, new ItemEntity { Id = Item3Id, Name = "", Price = 0 }),
                                    (33, new ItemEntity { Id = Item4Id, Name = "", Price = 0 }),
                                    (33, new ItemEntity { Id = Item5Id, Name = "", Price = 0 })
                }},
            };
            List<int> areas = new() { 1, 2, 3 };
            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock.Setup(x => x.Get(areas)).Returns(areaEntities);

            var extractorsEntities = new List<ExtractorEntity>()
            {
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 1, RPM = 2 },
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 2, RPM = 4 },
                new ExtractorEntity { Id = Guid.Empty, ExtractorType = ExtractorType.Mine, Level = 3, RPM = 6 }
            };

            var extractorsRepositoryMock = new Mock<IExtractorsRepository>();
            extractorsRepositoryMock.Setup(x => x.GetAll()).Returns(extractorsEntities);


            var service = new CalculatorService(areaRepositoryMock.Object, extractorsRepositoryMock.Object);

            #endregion Arrange
            #region Act
            var result = service.CalculateRPM(
                new CalcRequest
                (
                    new List<Extractor> {
                        new Extractor(1, 1, Guid.Empty, ExtractorType.Mine),
                        new Extractor(2, 2, Guid.Empty, ExtractorType.Mine),
                        new Extractor(3, 1, Guid.Empty, ExtractorType.Mine)
                    },
                    new List<Crafter>(),
                    new List<InventoryItem>(),
                    new List<TechLabBoost>() { TechLabBoost.ExtractingBoost }
                ));
            #endregion Act
            #region Assert
            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id), "Item 3 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item4Id), "Item 4 not included in result");
            Assert.IsTrue(result.Any(x => x.ItemId == Item5Id), "Item 5 not included in result");

            Assert.IsTrue(result.Any(x => x.ItemId == Item1Id && x.RPM == (1.0m + 4.0m) * 2), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item2Id && x.RPM == (0.5m) * 2), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item3Id && x.RPM == (0.5m + 0.66m) * 2), "Item 3 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item4Id && x.RPM == (0.66m) * 2), "Item 4 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.ItemId == Item5Id && x.RPM == (0.66m) * 2), "Item 5 doesn't have correct RPM");
            #endregion Assert
        }
    }
}
