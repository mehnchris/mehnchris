using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepVault.Data;
using RepVault.Models;

namespace RepVault.Controllers
{
    [Authorize]
    public class RepVaultGoalsController : Controller
    {
        private readonly RepVaultDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepVaultGoalsController(RepVaultDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge(); // Redirects to login page
            }

            var goals = _context.RepVaultGoals
                .Where(g => g.UserId == user.Id)
                .OrderBy(g => g.TargetDate)
                .ToList();

            return View(goals);
        }


        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepVaultGoal goal)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            goal.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.RepVaultGoals.Add(goal);
                await _context.SaveChangesAsync();

                TempData["GoalSuccess"] = "Goal created successfully!";
                return RedirectToAction("Index");
            }

            return View(goal);
        }


    }
}
