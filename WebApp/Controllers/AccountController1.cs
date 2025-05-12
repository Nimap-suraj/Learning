using System.Collections.Immutable;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.data;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController1 : Controller
    {
        private readonly MyAppContext _context;
        public AccountController1(MyAppContext context)
        {
            _context = context;
        }
        public IActionResult UsersPage()
        {
            return View(_context.userAccounts.ToListAsync());
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserAccount account = new UserAccount();
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Email = model.Email;
                account.UserName = model.UserName;
                account.Password = model.Password;


                try
                {

                    _context.userAccounts.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.UserName} Registration Successfully! ,Please Login Now! ";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter Correct email or password");
                    return View(model);
                }


                return View(model);
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               var user = _context.userAccounts.
                    Where(x => 
                    (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail) 
                    && x.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                        // Success,Create Cokkie
                        var claims =  new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.Email)
                        };
                    var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ClaimsIdentity));

                    return RedirectToAction("SecurePage");
                }

                else
                {
                    ModelState.AddModelError("", "Please enter Valid email/UserName or pasword!");
                }
                    return View(model);
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("UsersPage");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
