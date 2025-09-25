using EM.Core.Models.Entities;
using EM.Models;
using EM.Models.Entities;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class AssignedCompanyViewComponent : ViewComponent
    {
        private readonly IEmployeeService _employeeService;
        private readonly IGenericService<CompanyMetadata, Core.Models.Entities.Company> _gCompanyService;

        public AssignedCompanyViewComponent(IEmployeeService employeeService, IGenericService<CompanyMetadata, Core.Models.Entities.Company> gCompanyService)
        {
            _employeeService = employeeService;
            _gCompanyService = gCompanyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            string userId = Request.Cookies["userId"];
            ViewBag.UserId = userId;
            EmployeeMetadata employee = _employeeService.GetById(userId);
            ViewBag.Name = employee?.FullName;
            string companyId = (employee?.CompanyId ?? 0).ToString();
            IEnumerable<CompanyMetadata> companies = await _gCompanyService.GetAllAsync();
            ViewBag.Company = companies.Select
                (c => new SelectListItem()
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyName,
                    Selected = (companyId == c.CompanyId.ToString()) ? true : false
                });

            return View();
        }
    }
}
