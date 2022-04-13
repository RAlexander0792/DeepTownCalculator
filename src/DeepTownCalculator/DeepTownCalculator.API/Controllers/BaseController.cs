using Microsoft.AspNetCore.Mvc;

namespace DeepTownCalculator.API.Controllers
{
    public class BaseController<T> : Controller
    {
        protected readonly ILogger<T> logger;
        public BaseController(ILogger<T> logger)
        {
            this.logger = logger;
        }
    }
}
