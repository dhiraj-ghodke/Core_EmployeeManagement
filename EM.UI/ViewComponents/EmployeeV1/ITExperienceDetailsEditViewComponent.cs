using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class ITExperienceDetailsEditViewComponent : ViewComponent
    {
        private readonly IGenericService<EmployeeITExperienceMetadata, EM.Core.Models.Entities.EmployeeITExperience> _gITExperienceService;
        private readonly IGenericService<BatchMetadata, EM.Core.Models.Entities.Batch> _gBatchService;
        private readonly IGenericService<TeamMetadata, EM.Core.Models.Entities.Team> _gTeamService;
        private readonly IGenericService<EmployeeBankAccountMetadata, EM.Core.Models.Entities.EmployeeBankAccount> _gBankAccountService;

        public ITExperienceDetailsEditViewComponent(IGenericService<EmployeeITExperienceMetadata, Core.Models.Entities.EmployeeITExperience> gITExperienceService, IGenericService<BatchMetadata, EM.Core.Models.Entities.Batch> gBatchService, IGenericService<TeamMetadata, EM.Core.Models.Entities.Team> gTeamService, IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> gBankAccountService)
        {
            _gITExperienceService = gITExperienceService;
            _gBatchService = gBatchService;
            _gTeamService = gTeamService;
            _gBankAccountService = gBankAccountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = Request.Cookies["userId"];

            EmployeeITExperienceMetadata employeeITExperience = await _gITExperienceService.GetByUserIdAsync(e => e.UserId == userId);
            IEnumerable<BatchMetadata> batches = await _gBatchService.GetAllAsync();
            IEnumerable<TeamMetadata> teams = await _gTeamService.GetAllAsync();
            IEnumerable<EmployeeBankAccountMetadata> accounts = await _gBankAccountService.GetByUserIdAsync(u => u.UserId == userId, true);
            ViewBag.BatchIdList = new SelectList
                (batches?.ToList(), "BatchId", "BatchName");

            ViewBag.TeamIdList = new SelectList
                (teams?.ToList(), "TeamId", "TeamName");

            ViewBag.AccountIdList = new SelectList
                (accounts?.ToList(), "AccountId", "BankName");

            return View(employeeITExperience);
        }
    }
}
