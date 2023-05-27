using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MobileOperatorAppServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobileOperatorAppServer.Controllers
{
    public class LoginController : Controller
    {
        private Context context;

        public LoginController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                TempData["ErrorMessage"] = "Помилка! Введіть ім’я користувача та пароль";
                return RedirectToAction("Index");
            }

            AdminModel admin = context.Admins.FirstOrDefault(a => a.Username == Username);

            if (admin == null || admin.Password != Password)
            {
                try
                {
                    TempData["ErrorMessage"] = "Помилка! Введено некоректне ім’я користувача або пароль";
                }
                catch { }
                
                return RedirectToAction("Index");
            }

            var claims = new List<Claim> 
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            TempData["Username"] = Username;
            TempData["Password"] = Password;
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
