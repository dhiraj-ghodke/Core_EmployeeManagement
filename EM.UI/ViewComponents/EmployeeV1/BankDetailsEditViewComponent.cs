using EM.Core.Models.Entities;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class BankDetailsEditViewComponent : ViewComponent
    {
        private readonly IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> _gBankAccountService;
        public BankDetailsEditViewComponent(IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> gBankAccountService)
        {
            _gBankAccountService = gBankAccountService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = Request.Cookies["userId"];
             IEnumerable<EmployeeBankAccountMetadata> employeeBankAccount = await _gBankAccountService.GetByUserIdAsync(e => e.UserId == userId, true);

            return View(employeeBankAccount.ToList());
        }
    }
}
