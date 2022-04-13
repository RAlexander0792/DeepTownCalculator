using DeepTownCalculator.Application.Interfaces;
using DeepTownCalculator.Domain.Enums;
using DeepTownCalculator.Domain.Repositories;

namespace DeepTownCalculator.Application
{
    public record ItemRPM (int ItemId,decimal Produced,decimal RPM, decimal Reserve, decimal DepletedInMins, decimal DepletedInHours);
    public record Extractor (int Area, int Level, int ItemId,ExtractorType Type);
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
        public List<ItemRPM> CalculateRPM (CalcRequest request)
        {
            var dict = new Dictionary<Guid, ItemRPM> ();
            var rpmList = new List<ItemRPM>();

            var areas = areaRepository.Get(request.Extractors.Select(x => x.Area));
            var extractors = extractorsRepository.GetAll();



            return dict.ToList().Select(x => x.Value).ToList();
        }
    }
}