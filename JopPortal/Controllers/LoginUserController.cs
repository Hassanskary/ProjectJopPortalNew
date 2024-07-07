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
using Microsoft.AspNetCore.Mvc.Rendering;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace JopPortal.Controllers
{
    public class LoginUserController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _Host;
        public LoginUserController(AppDbContext context, IWebHostEnvironment Host)
        {
            this.context = context;
            _Host = Host;
        }

        public bool Check_login()
        {
            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            return Email == null;
        }

        public IActionResult Profile()
        {


            var Email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(Email))
            {
                return RedirectToAction("LogIn");
            }
            User user = context.user.FirstOrDefault(u => u.Email == Email);

            return View(user);
        }


        public IActionResult ViewCV(string filePath)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginUser");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound();
            }

            var cvPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CV", filePath);
            if (!System.IO.File.Exists(cvPath))
            {
                return NotFound();
            }

            var extension = Path.GetExtension(filePath).ToLower();
            string mimeType;
            switch (extension)
            {
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".doc":
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                default:
                    return BadRequest("Unsupported file format");
            }

            ViewBag.CVPath = Url.Content($"~/CV/{filePath}");
            ViewBag.MimeType = mimeType;
            ViewBag.Extension = extension;
            return View();
        }




        [HttpGet]
        public IActionResult LogIn()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult LogInSave(User Us)
        {
            HttpContext.Session.Clear();

            User user = context.user.FirstOrDefault(x => x.Email == Us.Email && x.Password == HashPassword(Us.Password));
            User isEmailExist = context.user.FirstOrDefault(x => x.Email == Us.Email);
            User isPassExist = context.user.FirstOrDefault(x => x.Password == HashPassword(Us.Password));


            if (user != null && user.Password == HashPassword(Us.Password))
            {
                HttpContext.Session.SetString("Email", Us.Email);
                //  ViewBag.IsAuthenticated = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {


                ModelState.Clear();
                if (isEmailExist == null)
                {
                    ModelState.AddModelError("Email", "This Email is not found");
                    return View("LogIn", Us);

                }
                else
                {
                    ModelState.AddModelError("Password", "Incorrect Password");
                    //ViewBag.IsAuthenticated = false;
                    return View("LogIn", Us);
                }
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {

            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpSave(User Us, IFormFile img_file, IFormFile file)
        {
            var isEmailExist = context.user.Any(x => x.Email == Us.Email);
            if (isEmailExist)
            {
                ModelState.AddModelError("Email", "Email is already exists");
                return View("SignUp", Us);
            }
            if (ModelState.IsValid)
            {
                if (Us.Password == Us.ConfirmPassword)
                {
                    Us.Password = HashPassword(Us.Password);
                    Us.ConfirmPassword = HashPassword(Us.ConfirmPassword);
                    if (img_file != null)
                    {
                        var photoExtension = Path.GetExtension(img_file.FileName).ToLower();
                        if (photoExtension != ".jpg" && photoExtension != ".png" && photoExtension != ".jpeg")
                        {
                            ModelState.AddModelError("PhotoPath", "Invalid Photo");
                            return View("SignUp", Us);
                        }
                        string path = Path.Combine(_Host.WebRootPath, "images");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        path = Path.Combine(path, img_file.FileName);
                        using (var Stream = new FileStream(path, FileMode.Create))
                        {
                            img_file.CopyTo(Stream);
                            Us.PhotoPath = img_file.FileName;
                        }
                    }

                    else
                    {
                        Us.PhotoPath = "default.png";
                    }
                    if (file != null)
                    {
                        var cvExtension = Path.GetExtension(file.FileName).ToLower();
                        if (cvExtension != ".pdf" && cvExtension != ".doc" && cvExtension != ".docx" && cvExtension != ".pptx")
                        {
                            ModelState.AddModelError("FilePath", "Invalid CV");
                            return View("SignUp", Us);
                        }

                        string cvPath = Path.Combine(_Host.WebRootPath, "CV");
                        if (!Directory.Exists(cvPath))
                        {
                            Directory.CreateDirectory(cvPath);
                        }

                        string cvFilePath = Path.Combine(cvPath, file.FileName);
                        using (var stream = new FileStream(cvFilePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            Us.FilePath = file.FileName;
                        }

                    }
                    context.user.Add(Us);
                    context.SaveChanges();
                    return RedirectToAction("LogIn");
                }
            }
            return View("SignUp", Us);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginUser");
            }

            if (id == null || context.user == null)
            {
                return NotFound();
            }

            var user = await context.user
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public IActionResult Edit(int? id)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginUser");
            }

            User user = context.user.FirstOrDefault(u => u.Id == id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit(User model, IFormFile img_file, IFormFile file)
        {
            User user = context.user.FirstOrDefault(u => u.Id == model.Id);
            string path = Path.Combine(_Host.WebRootPath, "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (img_file != null)
            {

                var fileExtension = Path.GetExtension(img_file.FileName).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg")
                {
                    ModelState.AddModelError("PhotoPath", " InValid Photo ");
                    return View("Edit", model);
                }
                path = Path.Combine(path, img_file.FileName);
                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    img_file.CopyTo(Stream);
                    user.PhotoPath = img_file.FileName;
                }
            }
            else
            {
                user.PhotoPath = user.PhotoPath??"default.png";
            }
            if (file != null)
            {
                string cvPath = Path.Combine(_Host.WebRootPath, "CV");

                if (!Directory.Exists(cvPath))
                {
                    Directory.CreateDirectory(cvPath);
                }

                var cvExtension = Path.GetExtension(file.FileName).ToLower();
                if (cvExtension != ".pdf" && cvExtension != ".doc" && cvExtension != ".docx" && cvExtension != ".pptx")
                {
                    ModelState.AddModelError("FilePath", "Invalid CV");
                    return View("Edit", model);
                }
                string cvFilePath = Path.Combine(cvPath, file.FileName);
                using (var stream = new FileStream(cvFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    user.FilePath = file.FileName;
                }
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.Age = model.Age;
            user.Gender = model.Gender;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            context.user.Update(user);
            context.SaveChanges();
            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult EditPass(int userId)
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginUser");
            }

            User user = context.user.FirstOrDefault(u => u.Id == userId);

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPass(User userPass,string currentPassword)
        {
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(currentPassword))
            {
                ViewData["currReq"]= "Current password is required";
                return View();
            }
            User user = context.user.FirstOrDefault(u => u.Email == email);

            if (HashPassword(currentPassword) == user.Password)
            {
                if (userPass.Password == userPass.ConfirmPassword)
                {

                    if (!string.IsNullOrEmpty(userPass.Password))
                    {
                        user.Password = HashPassword(userPass.Password);
                        user.ConfirmPassword = HashPassword(userPass.ConfirmPassword);

                    }
                    context.user.Update(user);
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
                return RedirectToAction("LogIn", "LoginUser");
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = context.user.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await context.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            context.user.Remove(user);
            await context.SaveChangesAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn");
        }

        public async Task<IActionResult> MyApplications()
        {
            if (Check_login())
            {
                return RedirectToAction("LogIn", "LoginUser");
            }

            var Email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(Email))
            {
                return RedirectToAction("LogIn");
            }

            var user = await context.user.Include(u => u.ApplyJobs).ThenInclude(aj => aj.Job)
                           .FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null)
            {
                return NotFound();
            }

            return View(user.ApplyJobs);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
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