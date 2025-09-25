using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class ResignationEditViewComponent : ViewComponent
    {
        private readonly IGenericService<EmployeeResignationMetadata, Core.Models.Entities.EmployeeResignation> _gEResignationService;

        public ResignationEditViewComponent(IGenericService<EmployeeResignationMetadata, Core.Models.Entities.EmployeeResignation> gEResignationService)
        {
            _gEResignationService = gEResignationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = Request.Cookies["userId"];
            EmployeeResignationMetadata employeeResignation = await _gEResignationService.GetByUserIdAsync(e => e.UserId == userId);

            return View(employeeResignation);
        }
    }
}
