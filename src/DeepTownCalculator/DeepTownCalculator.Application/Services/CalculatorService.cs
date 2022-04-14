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
    public record CalcRequest (IEnumerable<Extractor> Extractors, IEnumerable<Crafter> Crafters, IEnumerable<InventoryItem> Inventory);
    public class CalculatorService : ICalculatorService
    {
        private readonly IAreaRepository areaRepository;
        private readonly IExtractorsRepository extractorsRepository;
        public CalculatorService(IAreaRepository areaRepository, IExtractorsRepository extractorsRepository)
        {
            this.areaRepository = areaRepository;
            this.extractorsRepository = extractorsRepository;
        }
        public List<ItemResultEntity> CalculateRPM (CalcRequest request)
        {
            var resDict = new Dictionary<Guid, ItemResultEntity> ();

            CalculateExtractorsRPM(resDict, request.Extractors);


            return resDict.ToList().Select(x => x.Value).ToList();
        }

        private void CalculateExtractorsRPM(Dictionary<Guid, ItemResultEntity> resDict, IEnumerable<Extractor> extractors)
        {
            var coveredAreas = areaRepository.Get(extractors.Select(x => x.Area));
            var allExtractors = extractorsRepository.GetAll();

            foreach (var extractorReq in extractors)
            {
                var area = coveredAreas.First(x => x.AreaNumber == extractorReq.Area);
                var extractor = allExtractors.First(x => extractorReq.Level == x.Level && x.ExtractorType == extractorReq.Type);
                switch (extractorReq.Type)
                {
                    case ExtractorType.Mine:
                        foreach (var resource in area.Resources)
                            AddToDictionary(resDict, new ItemResultEntity { ItemId = resource.Item2.Id, RPM = ExtractorTypeMineRPMPerc(resource.Item1, extractor.RPM) });
                    break;
                    case ExtractorType.Chemistry:
                    case ExtractorType.Oil:
                        if (extractorReq.ItemId != Guid.Empty)
                            AddToDictionary(resDict, new ItemResultEntity { RPM = extractor?.RPM ?? 0, ItemId = extractorReq.ItemId});
                    break;
                }
            }

        }
        private void AddToDictionary(Dictionary<Guid, ItemResultEntity> resDict, ItemResultEntity ItemResultEntity)
        {
            if (resDict.TryGetValue(ItemResultEntity.ItemId, out ItemResultEntity? savedRPM))
            {
                savedRPM.RPM = ItemResultEntity.RPM;
            }
        }
        private decimal ExtractorTypeMineRPMPerc(int perc, decimal RPM)
        {
            //if (boosts.Any(e => e == 7)) rpmPercentage *= 2;
            return Decimal.Round(((decimal)perc / (decimal)100) * RPM, 2);
        }
    }
}