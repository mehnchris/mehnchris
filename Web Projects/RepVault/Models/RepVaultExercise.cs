using System.ComponentModel.DataAnnotations;

namespace RepVault.Models
{
    public class RepVaultExercise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ?Name { get; set; }

        public string ?MuscleGroup { get; set; }

        public bool IsCustom { get; set; } = false;
       // [Required]
       public int Sets { get; set; }
        //[Required]
        public int Reps { get; set; }
       // [Required]
        public float Weight { get; set; }
    }
}
