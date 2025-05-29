using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcProject.Models;

namespace MyMvcProject.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
                var IsAdmin = HttpContext.Session.GetString("IsAdmin");
                if (IsAdmin == "true")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                // ورود موفق
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                // ورود ناموفق، نمایش دوباره صفحه با پیام خطا
                ViewBag.Error = "نام کاربری یا رمز عبور اشتباه است.";
                return View();
            }
        }
        public IActionResult Dashboard()
        {
            var IsAdmin = HttpContext.Session.GetString("IsAdmin");
            if (IsAdmin == "true")
            {

                return View();
            }
else{ return RedirectToAction("Login", "Admin");}

        }
       


    }
    
        
}
