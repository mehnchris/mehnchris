using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepVault.Data;
using RepVault.Models;
using RepVault.ViewModels;
using System.Linq;

namespace RepVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RepVaultDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            RepVaultDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // Public homepage
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }

            return View(); // Views/Home/Index.cshtml (for unauthenticated users)
        }

        // Authenticated user dashboard
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            // Generate a dynamic time-based greeting
            var hour = DateTime.Now.Hour;
            var rng = new Random();

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

            string greeting = hour switch
            {
                >= 5 and < 12 => morningGreetings[rng.Next(morningGreetings.Length)],
                >= 12 and < 17 => afternoonGreetings[rng.Next(afternoonGreetings.Length)],
                >= 17 and < 22 => eveningGreetings[rng.Next(eveningGreetings.Length)],
                _ => nightGreetings[rng.Next(nightGreetings.Length)]
            };

            ViewBag.DynamicGreeting = greeting;

            // Load user-specific data
            var user = await _userManager.GetUserAsync(User);

            var latestWorkouts = _context.RepVaultWorkouts
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(w => w.Date)
                .Take(5)
                .ToList();

            var activeGoals = _context.RepVaultGoals
                .Where(g => g.UserId == user.Id && g.TargetDate >= DateTime.Today)
                .OrderBy(g => g.TargetDate)
                .ToList();

            var model = new DashboardViewModel
            {
                LatestWorkouts = latestWorkouts,
                ActiveGoals = activeGoals
            };

            return View(model); // Views/Home/Dashboard.cshtml
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
