using DeepTownCalculator.Application.Interfaces;
using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Enums;
using DeepTownCalculator.Domain.Repositories;

namespace DeepTownCalculator.Application
{
    public record Extractor (int Area, int Level, Guid ItemId,ExtractorType Type);
    public record Crafter (int RecipeId, int BuildingId);
    public record InventoryItem (int ItemId, int Quantity);
    public record Bot (int BuildingId, BotAction Action);
    public record CalcRequest (
        IEnumerable<Extractor> Extractors, 
        IEnumerable<Crafter> Crafters, 
        IEnumerable<InventoryItem> Inventory,
        IEnumerable<TechLabBoost> TechManagBoosts
        );
    public class CalculatorService : ICalculatorService
    {
        private readonly IAreaRepository areaRepository;
        private readonly IExtractorsRepository extractorsRepository;
        private readonly IRecipesRepository recipesRepository;
        public CalculatorService(IAreaRepository areaRepository, IExtractorsRepository extractorsRepository, IRecipesRepository recipesRepository)
        {
            this.areaRepository = areaRepository;
            this.extractorsRepository = extractorsRepository;
            this.recipesRepository = recipesRepository;
        }
        public List<ItemResultEntity> CalculateRPM (CalcRequest request)
        {
            var resDict = new Dictionary<Guid, ItemResultEntity> ();

            CalculateExtractorsRPM(resDict, request.Extractors, request.TechManagBoosts);

            return resDict.ToList().Select(x => x.Value).ToList();
        }

        private void CalculateExtractorsRPM(Dictionary<Guid, ItemResultEntity> resDict, IEnumerable<Extractor> extractors, IEnumerable<TechLabBoost> boosts)
        {
            var coveredAreas = areaRepository.Get(extractors.Select(x => x.Area));
            var allExtractors = extractorsRepository.GetAll();

            foreach (var extractorReq in extractors)
            {
                var area = coveredAreas.First(x => x.AreaNumber == extractorReq.Area);
                var extractor = allExtractors.First(x => extractorReq.Level == x.Level && x.ExtractorType == extractorReq.Type);
                CalculateExtractorRPMByType(resDict, extractorReq, area, extractor, boosts);
            }
        }
        private void CalculateExtractorRPMByType(Dictionary<Guid, ItemResultEntity> resDict, Extractor extractorReq, AreaEntity area, ExtractorEntity extractor, IEnumerable<TechLabBoost> boosts)
        {
            switch (extractorReq.Type)
            {
                case ExtractorType.Mine:
                    foreach (var resource in area.Resources)
                        AddToDictionary(resDict, new ItemResultEntity { ItemId = resource.Item2.Id, RPM = ExtractorTypeMineRPMPerc(resource.Item1, extractor.RPM, boosts.Any(x => x == TechLabBoost.ExtractingBoost)) });
                    break;
                case ExtractorType.Chemistry:
                case ExtractorType.Oil:
                    if (extractorReq.ItemId != Guid.Empty)
                        AddToDictionary(resDict, new ItemResultEntity { RPM = extractor?.RPM ?? 0, ItemId = extractorReq.ItemId });
                    break;
            }
        }
        private void AddToDictionary(Dictionary<Guid, ItemResultEntity> resDict, ItemResultEntity itemResultEntity)
        {
            if (resDict.TryGetValue(itemResultEntity.ItemId, out ItemResultEntity? savedRPM))
            {
                savedRPM.RPM += itemResultEntity.RPM;
            } else
            {
                resDict.Add(itemResultEntity.ItemId, itemResultEntity);
            }
        }
        private decimal ExtractorTypeMineRPMPerc(int perc, decimal RPM, bool techLabBoost)
        {
            var RPMperc = ((decimal)perc / (decimal)100) * RPM;
            if (techLabBoost) RPMperc *= 2;
            return Decimal.Round(RPMperc, 2);
        }
    }
}