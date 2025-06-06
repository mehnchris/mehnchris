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


            var morningGreetings = new[]
{
    "Rise and grind! It’s a new day to crush your goals 💥",
    "Let the reps begin — the early pump gets the gains ☀️",
    "Mornings are for warriors. Time to lift and conquer 🛡️"
};

            var afternoonGreetings = new[]
            {
    "Power through your afternoon — your gains won’t wait 🔥",
    "Lunch is fuel. Strength is built in sweat, not comfort 💪",
    "Midday motivation: show the iron who’s boss 🏋️"
};

            var eveningGreetings = new[]
            {
    "Finish strong — your future self will thank you 🏋️",
    "Evenings are built for discipline. Own your progress 💯",
    "No one regrets a workout. Let's end the day like a beast 🐺"
};

            var nightGreetings = new[]
            {
    "Even legends train after dark 🌙 No excuses.",
    "The grind doesn’t sleep — neither do your dreams ✨",
    "While others rest, you rise. Late-night sets incoming ⚡"
};

            var rng = new Random();
            if (hour >= 5 && hour < 12)
                greeting = morningGreetings[rng.Next(morningGreetings.Length)];
            else if (hour >= 12 && hour < 17)
                greeting = afternoonGreetings[rng.Next(afternoonGreetings.Length)];
            else if (hour >= 17 && hour < 22)
                greeting = eveningGreetings[rng.Next(eveningGreetings.Length)];
            else
                greeting = nightGreetings[rng.Next(nightGreetings.Length)];


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
