using jobPortal.Models;
using JopPortal.Data;
using JopPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JopPortal.Controllers
{
    public class JobController : Controller
    {
        readonly private AppDbContext context;
        public JobController(AppDbContext context) 
        {
            this.context = context;
        }

        public bool Check_login()
        {
            var Email = HttpContext.Session.GetString("CompanyEmail");
            ViewBag.EmailCompany = Email;
            return Email == null;
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            return View(new Job());
        }
        [HttpPost]
        public IActionResult Create(Job j)
        {
            ModelState.Remove("Company");
            if (ModelState.IsValid)
            {
                var Email = HttpContext.Session.GetString("CompanyEmail");

                var company = context.company.FirstOrDefault(x => x.CompanyEmail == Email);
                j.CompanyId = company.CompanyId;
                context.jop.Add(j);
                context.SaveChanges();
                return RedirectToAction("alljobs");
            }
            return View(j);
           
        }
        public IActionResult alljobs() 
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            var Email = HttpContext.Session.GetString("CompanyEmail");
            var company = context.company.FirstOrDefault(x => x.CompanyEmail == Email);
            List<Job> list = context.jop.ToList();
            List<Job> ans = new List<Job>();
            foreach(Job job in list)
            {
                if(job.CompanyId == company.CompanyId)
                    ans.Add(job);
            }
            return View(ans);
        }
        public IActionResult Details(int? id) 
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            var job = context.jop.FirstOrDefault(x => x.JobId == id);
            return View(job);
        }
        public IActionResult Edit(int? id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            Job job = context.jop.FirstOrDefault(c => c.JobId == id);
            return View(job);
        }

        [HttpPost]
        public IActionResult Edit(Job j)
        {
            Job job = context.jop.FirstOrDefault(c => c.JobId == j.JobId);
            job.JopLocation = j.JopLocation;
            job.JobCategory = j.JobCategory;
            job.JobDate = j.JobDate;
            job.JobDescription = j.JobDescription;
            job.AvailablePlaces = j.AvailablePlaces;
            job.JobType = j.JobType;
            job.JopSalary = j.JopSalary;
            context.SaveChanges();
            return RedirectToAction("alljobs");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            var job = context.jop.FirstOrDefault(x => x.JobId == id);
            return View(job);
        }
        [HttpPost]
        public IActionResult Delete(Job j)
        {
            var job = context.jop.FirstOrDefault(x => x.JobId == j.JobId);
            context.jop.Remove(job);
            context.SaveChanges();
            return RedirectToAction("alljobs");
        }
        public IActionResult ShowApplicants(int id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }

            var applicants = context.applyJop
                                    .Where(x => x.JobId == id) 
                                    .ToList();
            var user = context.user.ToList();
            var jp = context.jop.ToList();
            List<string> ans = new List<string>();
            List<JobCategory> ans2 = new List<JobCategory>();
            foreach(ApplyJob ap in applicants) 
            {
                int u_id = ap.UserId;
                int j_id = ap.JobId;
                foreach(Job j in jp)
                {
                    if (j.JobId == j_id) ans2.Add(j.JobCategory);
                }
                foreach(User u in user)
                {
                    if (u.Id == u_id) ans.Add(u.Name);
                }
            }
            ViewBag.uname = ans;
            ViewBag.jname = ans2;

            return View(applicants);
        }

        [HttpPost]
        public IActionResult AcceptApplication(int id)
        {
            var application = context.applyJop.FirstOrDefault(a => a.JobId == id);
            if (application != null)
            {
                application.State = State.Accepted;
                context.SaveChanges();
                // Redirect to a confirmation page or back to the list
                return RedirectToAction("alljobs"); // Adjust the redirection as needed
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult RejectApplication(int id)
        {
            var application = context.applyJop.FirstOrDefault(a => a.JobId == id);
            if (application != null)
            {
                application.State = State.Rejected;
                context.SaveChanges();
                // Redirect to a confirmation page or back to the list
                return RedirectToAction("alljobs"); // Adjust the redirection as needed
            }
            return NotFound();
        }
    }
}

