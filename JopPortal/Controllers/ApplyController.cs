using jobPortal.Models;
using JopPortal.Data;
using JopPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JopPortal.Controllers
{
    public class ApplyController : Controller
    {
        private readonly AppDbContext context;
        public ApplyController(AppDbContext context)
        {
            this.context = context;
        }
        public bool Check_login()
        {
            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            return Email == null;
        }
        /* public IActionResult Index()
         {
             return View();
         }*/

        public IActionResult ShowJob(int id)
        {
            
            var job = context.jop.FirstOrDefault(j => j.JobId == id);
            if (job == null)
            {
                return NotFound(); 
            }
            var Email = HttpContext.Session.GetString("Email");
            
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginUser");
            }

            ViewBag.EmailUser = Email;
            User user = context.user.FirstOrDefault(u => u.Email == Email);

            bool hasApplied = context.applyJop.Any(a => a.JobId == id && a.UserId == user.Id);

         
            ViewBag.HasApplied = hasApplied;

            var comp = context.company.ToList();
            string ans = "";
            foreach(Company cp in comp)
            {
                if (cp.CompanyId == job.CompanyId)
                {
                    ans = cp.CompanyName;
                    break;
                }
            }
            ViewBag.CompanyName = ans;
            return View(job); 
        }

        [HttpPost]
        public IActionResult ApplyForJob(int jobId)
        {

            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            if (string.IsNullOrEmpty(Email))
            {
                return RedirectToAction("LogIn");
            }
            User user = context.user.FirstOrDefault(u => u.Email == Email);

            var application = new ApplyJob
            {
                UserId = user.Id,
                JobId = jobId,
                File = user.File,
                FilePath = user.FilePath,
                State = State.Pending,
                Date = DateTime.Now
            };

            context.applyJop.Add(application);
            context.SaveChanges();

            return RedirectToAction("Explore_Jobs", "Home"); 
        }


        [HttpPost]
        public IActionResult CancelApplication(int jobId)
        {
            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            if (string.IsNullOrEmpty(Email))
            {
                return RedirectToAction("LogIn");
            }
            User user = context.user.FirstOrDefault(u => u.Email == Email);

            var application = context.applyJop.FirstOrDefault(a => a.JobId == jobId && a.UserId == user.Id);
            if (application == null)
            {
                return NotFound(); 
            }

            context.applyJop.Remove(application);
            context.SaveChanges();

            return RedirectToAction("Explore_Jobs", "Home");
        }
    }
}
