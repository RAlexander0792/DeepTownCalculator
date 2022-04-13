using Microsoft.AspNetCore.Mvc;

namespace DeepTownCalculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : BaseController<CalculatorController>
    {
        public CalculatorController(ILogger<CalculatorController> logger) : base(logger)
        {
        }
    }
}
