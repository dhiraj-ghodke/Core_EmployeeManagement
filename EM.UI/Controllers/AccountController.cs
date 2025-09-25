using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace EM.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                string roleName = "";
                UserModel model = _accountService.Login(login, out roleName);
                if (model != null)
                {
                    //save login info to cookie
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim(ClaimTypes.NameIdentifier, model.Id.ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "MyAppCookieAuth");
                    HttpContext.SignInAsync("MyAppCookieAuth", new ClaimsPrincipal(claimsIdentity));
                    
                    var userId = model?.Id;
                    Response.Cookies.Append("userId", userId, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(1) // optional
                    });
                    return RedirectToLocal(returnUrl);
                }

            }
            ModelState.AddModelError("", "Login Failed");
            return View(login);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {                
                bool status = _accountService.Register(model);
                if (status)
                {
                    return RedirectToAction("Login", "Account");
                }
                ModelState.AddModelError("", "Registration Failed");
            }
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("RedirectOnLogin", "EmployeeV1");
        }

        public IActionResult LogOff()
        {
            HttpContext.SignOutAsync("MyAppCookieAuth");
            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
