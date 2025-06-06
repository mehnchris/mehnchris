using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepVault.Data;
using RepVault.Models;

namespace RepVault.Controllers
{
    [Authorize]
    public class RepVaultWorkoutController : Controller
    {
        private readonly RepVaultDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepVaultWorkoutController(RepVaultDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /RepVaultWorkout/Log
        public IActionResult Log()
        {
            return View(new RepVaultWorkout
            {
                Date = DateTime.Now 
            });
        }

        public async Task<IActionResult> Progress(string exerciseName = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            // Get unique exercise names for dropdown
            var exercises = _context.RepVaultWorkouts
                .Where(w => w.UserId == user.Id)
                .Select(w => w.ExerciseName)
                .Distinct()
                .ToList();

            ViewBag.ExerciseList = new SelectList(exercises);
            ViewBag.SelectedExercise = exerciseName;

            if (string.IsNullOrEmpty(exerciseName))
            {
                ViewBag.Labels = new List<string>();
                ViewBag.Values = new List<float>();
                return View();
            }

            var chartData = _context.RepVaultWorkouts
                .Where(w => w.UserId == user.Id && w.ExerciseName == exerciseName)
                .OrderBy(w => w.Date)
                .Select(w => new
                {
                    Date = w.Date.ToString("MM/dd/yyyy"),
                    Weight = w.Weight
                })
                .ToList();

            ViewBag.Labels = chartData.Select(d => d.Date).ToList();
            ViewBag.Values = chartData.Select(d => d.Weight).ToList();

            return View();
        }




        // POST: /RepVaultWorkout/Log
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Log(RepVaultWorkout workout)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge(); // Redirect user to login if not authenticated
            }

            // Manually assign the user ID 
            workout.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.RepVaultWorkouts.Add(workout);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Workout logged successfully!";
                return RedirectToAction("Log");
            }
            // I want to display what went wrong if anything did
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            

            return View(workout);
        }
        // GET: /RepVaultWorkout/History
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var workouts = _context.RepVaultWorkouts
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(w => w.Date)
                .ToList();

            return View(workouts);
        }

        // GET: /RepVaultWorkout/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var workout = await _context.RepVaultWorkouts.FindAsync(id);

            if (workout == null || workout.UserId != user.Id)
                return Unauthorized();

            return View(workout);
        }

        // POST: /RepVaultWorkout/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RepVaultWorkout workout)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != workout.Id || workout.UserId != user.Id)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                _context.Update(workout);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Workout updated successfully!";
                return RedirectToAction("History");
            }

            return View(workout);
        }



    }
}
