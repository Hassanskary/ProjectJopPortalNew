using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace jobPortal.Models
{
    public enum Gender
    {
        Male, Female
    }
    public class checkAge : ValidationAttribute
    {
        public override bool IsValid(Object value)
        {
            if (value is int age)
            {
                return age >= 17;
            }
            return false;
        }
    }
    public class FindUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must consist of at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain a combination of upper case characters, lower case characters, and digits.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [checkAge(ErrorMessage=("this Age is Invalid"))]
        public int Age { get; set; }

        [Required]
        [StringLength(11)]
        [DisplayName("Phone Number")]
        [RegularExpression(@"01[0-2]\d{8}|015\d{8}", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }
        public string? PhotoPath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string? FilePath { get; set; }
       [NotMapped]
        public IFormFile? File { get; set; }

        public ICollection<ApplyJob>? ApplyJobs { get; set; }

    }
}
