using System.ComponentModel.DataAnnotations;

namespace RepVault.Models
{
    public class RepVaultWorkout
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = "Please enter the workout date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter the exercise name.")]
        public string? ExerciseName { get; set; }

        [Range(1, 100, ErrorMessage = "Sets must be greater than 0.")]
        public int Sets { get; set; }

        [Range(1, 100, ErrorMessage = "Reps must be greater than 0.")]
        public int Reps { get; set; }

        [Range(0.1, 2000, ErrorMessage = "Weight must be greater than 0.")]
        public float Weight { get; set; }

        [Required(ErrorMessage = "Please enter the muscle group.")]
        public string? MuscleGroup { get; set; }

        public string? Notes { get; set; }

        public bool IsCustomExercise { get; set; } = false;

        public virtual ApplicationUser ?User { get; set; }
    }
}
