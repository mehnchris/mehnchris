using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepVault.Models;

namespace RepVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var hour = DateTime.Now.Hour;
            string greeting;

            if (hour >= 5 && hour < 12)
            {
                greeting = "Rise and grind! It’s a new day to crush your goals ??";
            }
            else if (hour >= 12 && hour < 17)
            {
                greeting = "Power through your afternoon — your gains won’t wait ??";
            }
            else if (hour >= 17 && hour < 22)
            {
                greeting = "Finish strong — your future self will thank you ???";
            }
            else
            {
                greeting = "Even legends train after dark ?? No excuses.";
            }

            ViewBag.DynamicGreeting = greeting;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
