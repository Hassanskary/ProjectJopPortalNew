using JopPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JopPortal.Controllers
{
    public class CompanyHomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CompanyHomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            var Email = HttpContext.Session.GetString("CompanyEmail");
            ViewBag.EmailCompany = Email;
            if (Email == null)
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            return View();
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
    }
}
