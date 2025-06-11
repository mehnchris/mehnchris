using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepVault.Data;
using RepVault.Models;
using RepVault.Helpers;

using System.Web.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

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

        [HttpGet]
        public IActionResult ProgressChartPreview()
        {
            using var image = new Image<Rgba32>(400, 200);
            image.Mutate(ctx =>
            {
                ctx.Fill(Color.White);
                var barColor = Color.SkyBlue;
                int[] values = { 60, 100, 140, 120, 180 }; // Replace with real workout data later
                int barWidth = 40;

                for (int i = 0; i < values.Length; i++)
                {
                    int x = 50 + i * 60;
                    int barHeight = values[i];
                    ctx.Fill(barColor, new SixLabors.ImageSharp.Drawing.RectangularPolygon(x, 180 - barHeight, barWidth, barHeight));
                }
            });

            var ms = new MemoryStream();
            image.SaveAsPng(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms.ToArray(), "image/png");
        }





        // POST: /RepVaultWorkout/Log
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Log(RepVaultWorkout workout)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge(); // Ensure the user is authenticated

            workout.UserId = user.Id;

            if (!string.IsNullOrEmpty(workout.ExerciseName))
            {
                // Normalize input
                var normalized = WorkoutNameHelper.Normalize(workout.ExerciseName);

                // Look for existing workouts with similar names
                var userWorkouts = _context.RepVaultWorkouts
                    .Where(w => w.UserId == user.Id && w.ExerciseName != null)
                    .Select(w => new
                    {
                        Original = w.ExerciseName,
                        Canonical = WorkoutNameHelper.Normalize(w.ExerciseName)
                    })
                    .ToList();

                var matched = userWorkouts.FirstOrDefault(w => w.Canonical == normalized);

                if (matched != null)
                {
                    // Suggest reusing a similar existing name
                    TempData["WorkoutSuggestion"] = $"Did you mean \"{matched.Original}\"?";
                    workout.ExerciseName = matched.Original; // Apply the normalized name
                }
            }

            if (ModelState.IsValid)
            {
                _context.RepVaultWorkouts.Add(workout);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Workout logged successfully!";
                return RedirectToAction("Log");
            }

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
            var normalized = WorkoutNameHelper.Normalize(workout.ExerciseName);


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
