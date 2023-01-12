using InterShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace InterShop.Controllers
{
    public class AuthorizationController : Controller
    {
        InterShopContext db;
        List<User> users;
        
        public AuthorizationController(InterShopContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection keys)
        {
            User temp = db.users.Where(u => u.Login == keys["Login"].ToString() && u.Password == keys["Password"].ToString()).FirstOrDefault();
            if (temp != null)
            {
                string role = db.roles.Where(r => r.Id == temp.RoleId).AsNoTracking().FirstOrDefault().Name.ToString();
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, temp.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index", "CRUD", null);
            }
            return View("Index");
        }
        public IActionResult Registration()
        {
            return View(new User());
        }
        public IActionResult Check(User user)
        {
            if (ModelState.IsValid)
            {
                User temp = user;
                db.users.Add(temp);
                db.SaveChanges();
                return Redirect ("/CRUD/Index");
            }
            return View();
        }
    }
}
