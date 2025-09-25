using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class ExperienceDetailsEditViewComponent : ViewComponent
    {
        private readonly IGenericService<EmployeeExperieceMetadata, Core.Models.Entities.EmployeeExperiece> _gEExperienceService;
        private readonly IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> _gBankAccountService;

        public ExperienceDetailsEditViewComponent(IGenericService<EmployeeExperieceMetadata, Core.Models.Entities.EmployeeExperiece> gEExperienceService, IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> gBankAccountService)
        {
            _gEExperienceService = gEExperienceService;
            _gBankAccountService = gBankAccountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = Request.Cookies["userId"];

            IEnumerable<EmployeeExperieceMetadata> employeeExperience = await _gEExperienceService.GetByUserIdAsync(e => e.UserId == userId, true);
            IEnumerable<EmployeeBankAccountMetadata> accountIdList = await _gBankAccountService.GetByUserIdAsync(e => e.UserId == userId, true);


            ViewBag.AccountIdList = new SelectList
                (accountIdList?.ToList(), "AccountId", "BankName");

            return View(employeeExperience);
        }
    }
}
