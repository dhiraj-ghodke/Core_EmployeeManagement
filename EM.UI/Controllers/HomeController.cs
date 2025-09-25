using EM.Services;
using EM.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EM.UI.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            //change the name to user id
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //ViewBag.IsRegisteredEmployee = _userService.GetById(Convert.ToInt32(loggedInUserId));
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
