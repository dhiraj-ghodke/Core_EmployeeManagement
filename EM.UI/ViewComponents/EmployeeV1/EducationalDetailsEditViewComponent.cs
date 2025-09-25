using EM.Models.Entities;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class EducationalDetailsEditViewComponent : ViewComponent
    {
        private readonly IEmployeeService _eService;
        public EducationalDetailsEditViewComponent(IEmployeeService eService)
        {
            _eService = eService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
        string userId = Request.Cookies["userId"];
            var employeeEducation = _eService.GetEducation(userId);
            ViewBag.EducationTypeIdList = new SelectList
                (_eService.GetEducationType() , "EducationTypeId", "EducationName");

            
            return View(employeeEducation);
        }
    }
}
