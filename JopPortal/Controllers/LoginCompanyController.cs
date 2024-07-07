using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JopPortal.Data;
using JopPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using jobPortal.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace JopPortal.Controllers
{
    public class LoginCompanyController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _Host;
        public LoginCompanyController(AppDbContext context, IWebHostEnvironment Host)
        {
            this.context = context;
            _Host = Host;
        }
        public bool Check_login()
        {
            var Email = HttpContext.Session.GetString("CompanyEmail");
            ViewBag.EmailCompany = Email;
            return Email == null;
        }
        public IActionResult Profile()
        {
            var Email = HttpContext.Session.GetString("CompanyEmail");
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }
            Company company = context.company.FirstOrDefault(u => u.CompanyEmail == Email);

            return View(company);
        }




        [HttpGet]
        public IActionResult LogIn()
        {
            return View(new Company());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult LogInSave(Company Cp)
        {
            HttpContext.Session.Clear();

            Company company = context.company.FirstOrDefault(x => x.CompanyEmail == Cp.CompanyEmail && x.CompanyPassword == HashPassword(Cp.CompanyPassword));//check if email and password are match
            Company isEmailExist = context.company.FirstOrDefault(x => x.CompanyEmail == Cp.CompanyEmail);
            Company isPassExist = context.company.FirstOrDefault(x => x.CompanyPassword == HashPassword(Cp.CompanyPassword));

            if (company != null)
            {
                HttpContext.Session.SetString("CompanyEmail", Cp.CompanyEmail);
                // ViewBag.IsAuthenticated = true;
                return RedirectToAction("Index", "CompanyHome");

            }
            else
            {
                ModelState.Clear();
                if (isEmailExist == null)
                {
                    ModelState.AddModelError("CompanyEmail", "This Email is not found");
                    return View("LogIn", Cp);
                }

                else
                {
                    ModelState.AddModelError("CompanyPassword", "Incorrect Password");
                    // ViewBag.IsAuthenticated = false;
                    return View("LogIn", Cp);
                }
            }

        }


        [HttpGet]
        public IActionResult SignUp()
        {

            return View(new Company());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpSave(Company Cp, IFormFile img_file)
        {
            var isEmailExist = context.company.Any(x => x.CompanyEmail == Cp.CompanyEmail);
            if (isEmailExist)
            {
                ModelState.AddModelError("CompanyEmail", "Email is already exists");
                return View("SignUp", Cp);
            }

            if (ModelState.IsValid)
            {
                if (Cp.CompanyPassword == Cp.ConfirmPassword)
                {
                    Cp.CompanyPassword = HashPassword(Cp.CompanyPassword);
                    Cp.ConfirmPassword = HashPassword(Cp.ConfirmPassword);


                    if (img_file != null)
                    {
                        string path = Path.Combine(_Host.WebRootPath, "images");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (img_file != null)
                        {
                            path = Path.Combine(path, img_file.FileName);
                            using (var Stream = new FileStream(path, FileMode.Create))
                            {
                                img_file.CopyTo(Stream);
                                Cp.PhotoPath = img_file.FileName;
                            }


                            var fileExtension = Path.GetExtension(img_file.FileName).ToLower();
                            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                            {
                                ModelState.AddModelError("PhotoPath", " InValid Photo ");
                                return View("SignUp", Cp);
                            }
                        }
                    }
                    else
                    {
                       
                        Cp.PhotoPath = "default.png";
                    }
                    context.company.Add(Cp);
                    context.SaveChanges();
                    return RedirectToAction("LogIn");
                }
            }
            return View("SignUp", Cp);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }

            if (id == null || context.company == null)
            {
                return NotFound();
            }

            var company = await context.company
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public IActionResult Edit(int? id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }

            Company company = context.company.FirstOrDefault(c => c.CompanyId == id);
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit(Company model, IFormFile img_file)
        {
            Company company = context.company.FirstOrDefault(c => c.CompanyId == model.CompanyId);
            string path = Path.Combine(_Host.WebRootPath, "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (img_file != null)
            {
                path = Path.Combine(path, img_file.FileName);
                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    img_file.CopyTo(Stream);
                    company.PhotoPath = img_file.FileName;
                }

                var fileExtension = Path.GetExtension(img_file.FileName).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError("PhotoPath", " InValid Photo ");
                    return View("Edit", model);
                }

            }
            else
            {
                company.PhotoPath = company.PhotoPath?? "default.png";
            }

            company.CompanyName = model.CompanyName;
            company.CompanyEmail = model.CompanyEmail;
            company.CompanyDescription = model.CompanyDescription;

            context.company.Update(company);

            context.SaveChanges();
            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult EditPass(int CompanyId)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }

            Company Company = context.company.FirstOrDefault(c => c.CompanyId == CompanyId);

            return View(Company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPass(Company comp,string currentPassword)
        {
            string email = HttpContext.Session.GetString("CompanyEmail");
            if (string.IsNullOrEmpty(currentPassword))
            {
                ViewData["currReq"] = "Current password is required";
                return View();
            }
            Company company = context.company.FirstOrDefault(c => c.CompanyEmail == email);

            if (HashPassword(currentPassword) == company.CompanyPassword)
            {
                if (comp.CompanyPassword == comp.ConfirmPassword)
                {
                    if (!string.IsNullOrEmpty(comp.CompanyPassword))
                    {
                        company.CompanyPassword = HashPassword(comp.CompanyPassword);
                        company.ConfirmPassword = HashPassword(comp.ConfirmPassword);

                    }
                    context.company.Update(company);
                    context.SaveChanges();
                    return RedirectToAction("Profile");
                }
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
            }

                return View();
            }


        public IActionResult DeleteConfirmation(int? id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginCompany");
            }

            if (id == null)
            {
                return NotFound();
            }

            var comp = context.company.FirstOrDefault(m => m.CompanyId == id);
            if (comp == null)
            {
                return NotFound();
            }

            return View(comp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comp = await context.company.FindAsync(id);
            if (comp == null)
            {
                return NotFound();
            }
            context.company.Remove(comp);
            await context.SaveChangesAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "CompanyHome");
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
