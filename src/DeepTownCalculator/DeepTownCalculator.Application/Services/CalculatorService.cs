using DeepTownCalculator.Application.Interfaces;

namespace DeepTownCalculator.Application
{
    public record ItemRPM (int ItemId,decimal Produced,decimal RPM, decimal Reserve, decimal DepletedInMins, decimal DepletedInHours);
    public record Extractor (int Area, int Level, int ItemId, int ExtractorType);
    public record CalcRequest (IEnumerable<Extractor> Extractors);
    public class CalculatorService : ICalculatorService
    {
        public List<ItemRPM> CalculateRPM (CalcRequest request)
        {
            var rpmList = new List<ItemRPM>();



            return rpmList;
        }
    }
}