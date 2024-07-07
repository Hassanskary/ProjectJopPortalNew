using JopPortal.Data;
using JopPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JopPortal.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        private readonly AppDbContext context;
        public HomeController(AppDbContext context)
        {
            this.context = context;
        }



        public IActionResult Index()
        {

            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            /*if(Email == null)
            {
                return RedirectToAction("LogIn", "LoginUser");
            }*/
            return View(new Search());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Explore_Jobs()
        {
            /*var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            if (Email == null)
            {
                return RedirectToAction("LogIn", "LoginUser");
            }
            */

            var jobs = context.jop.ToList();

            //
            var comp = context.company.ToList();
            List <string> lst = new List<string>();
            foreach (var job in jobs)
            {
                int id = job.CompanyId;
                foreach (var cp in comp)
                {
                    if (cp.CompanyId == id) lst.Add(cp.CompanyName);
                }
            }
            ViewBag.names = lst;
            //

            return View(jobs);
        }

        public IActionResult Filtering_Jobs()
        {
            return View(new Search());
        }

        public IActionResult Show_results(Search search)
        {

            var jobs = context.jop.AsEnumerable();

            if (search.Category != J_Category.Any)
            {
                jobs = jobs.Where(j => (int)j.JobCategory + 1 == (int)search.Category);
            }
            if (search.Type != J_Type.Any)
            {
                jobs = jobs.Where(j => (int)j.JobType + 1 == (int)search.Type);
            }
            if (search.Location != J_Location.Any)
            {
                jobs = jobs.Where(j => (int)j.JopLocation == (int)search.Location);
            }
            if (search.Min_Salary > 0)
            {
                jobs = jobs.Where(j => (int)j.JopSalary >= search.Min_Salary);
            }
            if (search.Max_Salary > 0)
            {
                jobs = jobs.Where(j => (int)j.JopSalary <= search.Max_Salary);
            }

            //
            var comp = context.company.ToList();
            List<string> lst = new List<string>();
            foreach (var job in jobs)
            {
                int id = job.CompanyId;
                foreach (var cp in comp)
                {
                    if (cp.CompanyId == id) lst.Add(cp.CompanyName);
                }
            }
            ViewBag.names = lst;
            //

            return View(jobs.ToList());
        }
    }
}
