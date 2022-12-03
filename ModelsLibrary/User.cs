using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? EmailAddress { get; set; }

        public double PercentageMod { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? UserImageLink { get; set; }

        public ICollection<Budget>? Budgets { get; set; } 
    }
}
