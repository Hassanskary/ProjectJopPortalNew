using jobPortal.Models;
using JopPortal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jobPortal.Models
{
    public enum State
    {
        Pending, Accepted, Rejected
    }
    public class ApplyJob
    {
		public int UserId { get; set; }
        public int JobId { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public State State { get; set; }
        public User User { get; set; }
        public Job Job { get; set; }
    }
}
