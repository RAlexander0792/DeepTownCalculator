using DeepTownCalculator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeepTownCalculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : BaseController<CalculatorController>
    {
        private readonly ICalculatorService calculatorService;
        public CalculatorController(ILogger<CalculatorController> logger, ICalculatorService calculatorService) : base(logger)
        {
            this.calculatorService = calculatorService;
        }
    }
}
