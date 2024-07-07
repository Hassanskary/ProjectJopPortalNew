using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JopPortal.Models
{

    public class FindCompany
    {
        [Required]
        [DisplayName("Email")]
        public string CompanyEmail { get; set; }
        [Required]
        [DisplayName("Password")]
        public string CompanyPassword { get; set; }
    }
    public class Company
    {

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Email is required")]
        [EmailAddress]
        [DisplayName("Email")]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must consist of at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain a combination of upper case characters, lower case characters, and digits.")]
        [DisplayName("Password")]
        public string CompanyPassword { get; set; }

        [Compare("CompanyPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [DisplayName("Name")]
        public string CompanyName { get; set; }

        public string? PhotoPath { get; set; }
        [NotMapped]
        [DisplayName("Photo")]
        public IFormFile? CompanyPhoto { get; set; }

        [Required(ErrorMessage = "Company Description is required")]
        [DisplayName("Description")]

        public string? CompanyDescription { get; set; }

        public virtual ICollection<Job>? Jobs { get; set; }


    }
}
