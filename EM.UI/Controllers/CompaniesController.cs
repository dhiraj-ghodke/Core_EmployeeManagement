using EM.Core.Models.Entities;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EM.UI.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly IGenericService<CompanyMetadata, EM.Core.Models.Entities.Company> _gCompanyService;
        private readonly IGenericService<UserModel, AspNetUser> _gUserService;

        public CompaniesController(IGenericService<CompanyMetadata, Core.Models.Entities.Company> gCompanyService, IGenericService<UserModel, AspNetUser> gUserService)
        {
            _gCompanyService = gCompanyService;
            _gUserService = gUserService;
        }

        // GET: Companies
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Index()
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CompanyMetadata> companies = await _gCompanyService.GetAllAsync();

            if (User.IsInRole("Super Admin"))
            {
                return View(companies.ToList());
            }

            return View(companies.ToList().Where(c => c.CreatedBy.Equals(loggedInUserId)));
        }


        // GET: Companies/Create
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Create()
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<UserModel> users = await _gUserService.GetAllAsync();
            ViewBag.CreatedBy = new SelectList
                (users.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            return View();
        }

        // GET: Companies/Details/5
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CompanyMetadata company = await _gCompanyService.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            UserModel user = await _gUserService.GetByIdAsync(company.CreatedBy);
            ViewBag.CompanyCreator = user.Email;
            return View(company);
        }


        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Create(CompanyMetadata company)
        {
            if (ModelState.IsValid)
            {
                company.CreatedDate = DateTime.Now;

                //db.Companies.Add(company);
                //db.SaveChanges();
                await _gCompanyService.AddAsync(company);
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(await _gUserService.GetAllAsync(), "Id", "Email", company.CreatedBy);
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CompanyMetadata company = await _gCompanyService.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewBag.CreatedBy = new SelectList(await _gUserService.GetAllAsync(), "Id", "Email", company.CreatedBy);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Edit(CompanyMetadata company)
        {
            if (ModelState.IsValid)
            {
                await _gCompanyService.UpdateAsync(company.CompanyId, company);
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(await _gUserService.GetAllAsync(), "Id", "Email", company.CreatedBy);
            return View(company);
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CompanyMetadata company = await _gCompanyService.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            UserModel user = await _gUserService.GetByIdAsync(company.CreatedBy);
            ViewBag.CompanyCreator = user.Email;
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _gCompanyService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
