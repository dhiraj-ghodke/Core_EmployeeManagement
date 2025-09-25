using EM.Core.Models.Entities;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EM.UI.Controllers
{
    [Authorize(Roles = "HR,Super Admin")]

    public class UserAdminController : Controller
    {
        private readonly IGenericService<UserModel, AspNetUser> _gUserService;
        private readonly IAccountService _accountService;

        public UserAdminController(IGenericService<UserModel, AspNetUser> gUserService, IAccountService accountService)
        {
            _gUserService = gUserService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UserModel> users = await _gUserService.GetAllAsync();
            return View(users?.ToList());
        }
        [HttpGet]
        public ActionResult UserDetails(string email)
        {
            //Session.Remove("VHaaShUserEmail");
            //Session["VHaaShUserEmail"] = email;
            UserModel user = _accountService.GetByEmail(email);
            var userId = user?.Id;
            Response.Cookies.Append("userId", userId, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1) // optional
            });
            //HttpCookie userInfo = new HttpCookie("userId", userId);
            //userInfo.Expires = DateTime.Now.AddDays(1);
            //Response.Cookies.Add(userInfo);

            return RedirectToAction("Home", "EmployeeV1");
        }

        //
        // GET: /Users/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null");
            }
            UserModel user = await _gUserService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: /Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return BadRequest();
                }

                UserModel user = await _gUserService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                await _gUserService.DeleteAsync(id);
                UserModel user1 = await _gUserService.GetByIdAsync(id);
                if (user1 != null)
                {
                    ModelState.AddModelError("", "Could not delete user");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

    