using JopPortal.Models;
using Microsoft.CodeAnalysis;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace JopPortal.Models
{
    public enum JobCategory
    {
        Customer_Service,
        Sales,
        Software_Development,
        Marketing,
        Instructor,
        Education,
        Media,
        Medical
    }
    public enum JobType
    {
        Full_Time,
        Internship,
        Freelance,
        Part_Time,
    }
    public enum Location
    {
        General,
        Alexandria,
        Aswan,
        Asyut,
        Beheira,
        BeniSuef,
        Cairo,
        Dakahlia,
        Damietta,
        Faiyum,
        Gharbia,
        Giza,
        Ismailia,
        KafrElSheikh,
        Luxor,
        Matruh,
        Minya,
        Monufia,
        NewValley,
        NorthSinai,
        PortSaid,
        Qalyubia,
        Qena,
        RedSea,
        Sharqia,
        Sohag,
        SouthSinai,
        Suez
    }
    public class Job
    {
        public int JobId { get; set; }
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Jop Category is required")]
        [DisplayName("Job Category")]
        public JobCategory JobCategory { get; set; }
        [DisplayName("Job Type")]

        [Required(ErrorMessage = "Jop Type is required")]
        public JobType JobType { get; set; }

        [Required(ErrorMessage = "Jop Location is required")]
        [DisplayName("Job Location")]
        public Location JopLocation { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid JopSalary")]
        [Required(ErrorMessage = "Jop Salary is required")]
        [DisplayName("Job Salary")]
        public int JopSalary { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid AvailablePlaces")]

        [Required(ErrorMessage = "AvailablePlaces is required")]
        [DisplayName("Available Places")]
        public int AvailablePlaces { get; set; }
        [DisplayName("Post Date")]

        public DateTime JobDate { get; set; } = DateTime.Now;
        [DisplayName("Job Description")]
        public string? JobDescription { get; set; }
        [DisplayName("Job Status")]
       // public string? JobStatus { get; set; }

        public Company? Company { get; set; }
    }
}
