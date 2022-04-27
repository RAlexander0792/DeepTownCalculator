using DeepTownCalculator.Application;
using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Enums;
using DeepTownCalculator.Domain.Repositories;
using DeepTownCalculator.UnitTests.EntityGenerators;
using DeepTownCalculator.UnitTests.Mockers;
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
            var recipesRepository = new Mock<IRecipesRepository>();
            var Item1Id = Guid.NewGuid();
            var Item2Id = Guid.NewGuid();
            var Item3Id = Guid.NewGuid();

            List<AreaEntity> areaEntities = new List<AreaEntity>() {
                new AreaEntity { 
                        Id = 1,
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


            var service = new CalculatorService(areaRepositoryMock.Object, extractorsRepositoryMock.Object, recipesRepository.Object);
            
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
            Assert.IsTrue(result.Any(x => x.Id == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item3Id), "Item 3 not included in result");

            Assert.IsTrue(result.Any(x => x.Id == Item1Id && x.RPM == 1.0m), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item2Id && x.RPM == 0.5m), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item3Id && x.RPM == 0.5m), "Item 3 doesn't have correct RPM");

            #endregion Assert
        }
        [Test]
        public void CalculateRPM_Excavators_Only3Mines_Ok()
        {
            #region Arrange
            List<ItemEntity> items = ItemGen.Generate(5);
            var area1Resources = new List<(int, Guid)> { (25, items[0].Id), (25, items[1].Id), (50, items[2].Id) };
            var area2Resources = new List<(int, Guid)> { (100, items[0].Id) };
            var area3Resources = new List<(int, Guid)> { (33, items[2].Id), (33, items[3].Id), (33, items[4].Id) };
            var areaRepositoryMock = new AreaRepositoryMocker().WithItems(items)
                .WithArea(1, area1Resources)
                .WithArea(2, area2Resources)
                .WithArea(3, area3Resources)
                .SetupGet(new int[] { 1,2,3 }).Build();

            var extractorsRepositoryMock = new ExtractorsRepositoryMocker().WithMine(1, 2).WithMine(2, 4).WithMine(3, 6).Build();

            var recipesRepository = new Mock<IRecipesRepository>();
            var service = new CalculatorService(areaRepositoryMock, extractorsRepositoryMock, recipesRepository.Object);

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
            Assert.IsTrue(result.Any(x => x.Id == items[0].Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == items[1].Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == items[2].Id), "Item 3 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == items[3].Id), "Item 4 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == items[4].Id), "Item 5 not included in result");

            Assert.IsTrue(result.Any(x => x.Id == items[0].Id && x.RPM == 0.5m + 4.0m), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == items[1].Id && x.RPM == 0.5m), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == items[2].Id && x.RPM == 0.5m + 0.66m), "Item 3 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == items[3].Id && x.RPM == 0.66m), "Item 4 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == items[4].Id && x.RPM == 0.66m), "Item 5 doesn't have correct RPM");
            #endregion Assert
        }

        [Test]
        public void CalculateRPM_Excavators_3Mines_ExtractorBoostOk()
        {
            #region Arrange
            var recipesRepository = new Mock<IRecipesRepository>();


            var Item1Id = Guid.NewGuid();
            var Item2Id = Guid.NewGuid();
            var Item3Id = Guid.NewGuid();
            var Item4Id = Guid.NewGuid();
            var Item5Id = Guid.NewGuid();

            List<AreaEntity> areaEntities = new List<AreaEntity>() {
                new AreaEntity {
                        Id = 1,
                        AreaNumber = 1,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (50, new ItemEntity { Id = Item1Id, Name = "", Price = 0 }),
                                    (25, new ItemEntity { Id = Item2Id, Name = "", Price = 0 }),
                                    (25, new ItemEntity { Id = Item3Id, Name = "", Price = 0 })
                }},
                new AreaEntity {
                        Id = 2,
                        AreaNumber = 2,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (100, new ItemEntity { Id = Item1Id, Name = "", Price = 0 }),
                }},
                new AreaEntity {
                        Id = 3,
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


            var service = new CalculatorService(areaRepositoryMock.Object, extractorsRepositoryMock.Object, recipesRepository.Object);

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
            Assert.IsTrue(result.Any(x => x.Id == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item3Id), "Item 3 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item4Id), "Item 4 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item5Id), "Item 5 not included in result");

            Assert.IsTrue(result.Any(x => x.Id == Item1Id && x.RPM == (1.0m + 4.0m) * 2), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item2Id && x.RPM == (0.5m) * 2), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item3Id && x.RPM == (0.5m + 0.66m) * 2), "Item 3 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item4Id && x.RPM == (0.66m) * 2), "Item 4 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item5Id && x.RPM == (0.66m) * 2), "Item 5 doesn't have correct RPM");
            #endregion Assert
        }
        [Test]
        public void CalculateRPM_Excavators_1Mines_1CrafterOk()
        {
            #region Arrange
            var Item1Id = Guid.NewGuid();
            var Item2Id = Guid.NewGuid();
            var Item3Id = Guid.NewGuid();
            var Item4Id = Guid.NewGuid();
            var recipesRepository = new Mock<IRecipesRepository>();
            List<RecipeEntity> recipeEntities = new () { new RecipeEntity { RecipeId = 1, 
                Ingredients =  new List<(int,ItemEntity)> { ( 2, new ItemEntity { Id = Item1Id } ) },
                Products = new List<(int, ItemEntity)> { (1, new ItemEntity { Id = Item4Id } ) }
            } };
            recipesRepository.Setup(x => x.Get(1)).Returns(recipeEntities);

            List<AreaEntity> areaEntities = new List<AreaEntity>() {
                new AreaEntity {
                        Id = 1,
                        AreaNumber = 1,
                        Oil = false,
                        Resources = new List<(int, ItemEntity)> () {
                                    (50, new ItemEntity { Id = Item1Id, Name = "", Price = 0 }),
                                    (25, new ItemEntity { Id = Item2Id, Name = "", Price = 0 }),
                                    (25, new ItemEntity { Id = Item3Id, Name = "", Price = 0 })
                }}
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


            var service = new CalculatorService(areaRepositoryMock.Object, extractorsRepositoryMock.Object, recipesRepository.Object);

            #endregion Arrange
            #region Act
            var result = service.CalculateRPM(
                new CalcRequest
                (
                    new List<Extractor> {
                        new Extractor(1, 2, Guid.Empty, ExtractorType.Mine)
                    },
                    new List<Crafter>() { new Crafter (1, 1) },
                    new List<InventoryItem>(),
                    new List<TechLabBoost>()
                ));
            #endregion Act
            #region Assert
            Assert.IsTrue(result.Any(x => x.Id == Item1Id), "Item 1 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item2Id), "Item 2 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item3Id), "Item 3 not included in result");
            Assert.IsTrue(result.Any(x => x.Id == Item4Id), "Item 4 not included in result");

            Assert.IsTrue(result.Any(x => x.Id == Item1Id && x.RPM == (1.0m + 4.0m)), "Item 1 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item2Id && x.RPM == 0.5m), "Item 2 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item3Id && x.RPM == 0.5m), "Item 3 doesn't have correct RPM");
            Assert.IsTrue(result.Any(x => x.Id == Item4Id && x.RPM == 2), "Item 4 doesn't have correct RPM");
            #endregion Assert
        }
    }
}
