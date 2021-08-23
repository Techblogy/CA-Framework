using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CAF.Core.Utilities;

namespace CAF.UI.WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppSettings _appSettings;

        public HomeController(ILogger<HomeController> logger, IAppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
