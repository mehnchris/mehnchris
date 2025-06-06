using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepVault.Models
{
    public class RepVaultGoal
    {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public string ?GoalTitle { get; set; }

        public string? Description { get; set; }

        [Required]
        public float TargetValue { get; set; }

        [Required]
        public string ?Unit { get; set; } // e.g. "lbs", "reps"

        [Required]
        public DateTime TargetDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ?User { get; set; }
    }
}
