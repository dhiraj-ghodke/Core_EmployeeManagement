using EM.Core.Models.Entities;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Threading.Tasks;

namespace EM.UI.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class RolesAdminController : Controller
    {
        private readonly IGenericService<RoleViewModel, AspNetRole> _gRoleService;
        private readonly IGenericService<UserModel, AspNetUser> _gUserService;
        private readonly IGenericService<UserRoleViewModel, UserRole> _gUserRoleService;

        public RolesAdminController(IGenericService<RoleViewModel, AspNetRole> gRoleService, IGenericService<UserModel, AspNetUser> gUserService, IGenericService<UserRoleViewModel, UserRole> gUserRoleService)
        {
            _gRoleService = gRoleService;
            _gUserService = gUserService;
            _gUserRoleService = gUserRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<RoleViewModel> roles = await _gRoleService.GetAllAsync();
            return View(roles);
        }
        //
        // GET: /Roles/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            RoleViewModel role = await _gRoleService.GetByIdAsync(id);
            // Get the list of Users in this Role
            //List<UserModel> users = new List<UserModel>();

            //IEnumerable<UserModel> registeredUsers = await _gUserService.GetAllAsync();
            IEnumerable<UserModel> registeredUsers = from u in await _gUserService.GetAllAsync()
                                  join ur in await _gUserRoleService.GetAllAsync()
                                  on u.Id equals ur.UserId
                                  where ur.RoleId == id
                                  select u;
            // Get the list of Users in this Role
            //foreach (var user in registeredUsers.ToList())
            //{
            //    if (user.role)
            //    {
            //        users.Add(user);
            //    }
            //}

            //ViewBag.Users = users;
            ViewBag.Users = registeredUsers.ToList();
            //ViewBag.UserCount = users.Count();
            ViewBag.UserCount = registeredUsers.Count();
            return View(role);
        }

        //
        // GET: /Roles/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                //var role = new IdentityRole(roleViewModel.Name);
                //var roleresult = await RoleManager.CreateAsync(role);
                //if (!roleresult.Succeeded)
                //{
                //    ModelState.AddModelError("", roleresult.Errors.First());
                //    return View();
                //}
                //return RedirectToAction("Index");
                IEnumerable<RoleViewModel> roles =  await _gRoleService.GetAllAsync();
                if(roles.Any(u => u.Id == roleViewModel.Id))
                {
                    ModelState.AddModelError("", "Id alreay exists");
                    return View (roleViewModel);
                }
                if(roles.Any(u => u.Name == roleViewModel.Name))
                {
                    ModelState.AddModelError("", $"{roleViewModel.Name} role already exists.");
                    return View(roleViewModel);
                }
                await _gRoleService.AddAsync(roleViewModel);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Enter valid id and role");
            return View(roleViewModel);
        }


        //
        // GET: /Roles/Edit/Admin
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            RoleViewModel role = await _gRoleService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            //RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(role);
        }
        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {

                //var role = await RoleManager.FindByIdAsync(roleModel.Id);
                //role.Name = roleModel.Name;
                //await RoleManager.UpdateAsync(role);
                IEnumerable<RoleViewModel> roles = await _gRoleService.GetAllAsync();

                if (roles.Any(u => u.Name == roleModel.Name))
                {
                    ModelState.AddModelError("", $"{roleModel.Name} role already exists.");
                    return View(roleModel);
                }
                await _gRoleService.UpdateAsync(roleModel.Id,roleModel);
                return RedirectToAction("Index");

            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            RoleViewModel role = await _gRoleService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }
        //
        // POST: /Roles/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string? deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var role = await _gRoleService.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                //IdentityResult result;
                //if (deleteUser != null)
                //{
                //    result = await RoleManager.DeleteAsync(role);
                //}
                //else
                //{
                //    result = await RoleManager.DeleteAsync(role);
                //}
                //if (!result.Succeeded)
                //{
                //    ModelState.AddModelError("", result.Errors.First());
                //    return View();
                //}
                await _gRoleService.DeleteAsync(role.Id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
