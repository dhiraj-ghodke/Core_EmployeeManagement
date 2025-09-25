using EM.Core.Models;
using EM.Core.Models.Entities;
using EM.Models;
using EM.Models.Entities;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EM.UI.Controllers
{
    public class EmployeeV1Controller : Controller
    {
        private readonly IEmployeeService _eService;
        private readonly IGenericService<EmployeeMetadata, Employee> _gEmployeeService;
        private readonly IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> _gEBankAccountService;
        private readonly IGenericService<EmployeeExperieceMetadata, Core.Models.Entities.EmployeeExperiece> _gEExperienceService;
        private readonly IGenericService<EmployeeITExperienceMetadata, Core.Models.Entities.EmployeeITExperience> _gEITExperienceservice;
        private readonly IGenericService<BatchMetadata, EM.Core.Models.Entities.Batch> _gBatchService;
        private readonly IGenericService<TeamMetadata, EM.Core.Models.Entities.Team> _gTeamService;
        private readonly IGenericService<EmployeeResignationMetadata, Core.Models.Entities.EmployeeResignation> _gEResignationService;

        public EmployeeV1Controller(IEmployeeService eService, IGenericService<EmployeeMetadata, Employee> gEmployeeService, IGenericService<EmployeeBankAccountMetadata, Core.Models.Entities.EmployeeBankAccount> gEBankAccountService, IGenericService<EmployeeExperieceMetadata, Core.Models.Entities.EmployeeExperiece> gEExperienceService, IGenericService<EmployeeITExperienceMetadata, Core.Models.Entities.EmployeeITExperience> gEITExperienceservice, IGenericService<BatchMetadata, EM.Core.Models.Entities.Batch> gBatchService, IGenericService<TeamMetadata, EM.Core.Models.Entities.Team> gTeamService, IGenericService<EmployeeResignationMetadata, Core.Models.Entities.EmployeeResignation> gEResignationService)
        {
            _eService = eService;
            _gEmployeeService = gEmployeeService;
            _gEBankAccountService = gEBankAccountService;
            _gEExperienceService = gEExperienceService;
            _gEITExperienceservice = gEITExperienceservice;
            _gBatchService = gBatchService;
            _gTeamService = gTeamService;
            _gEResignationService = gEResignationService;
        }
        [HttpGet]
        public ActionResult RedirectOnLogin()
        {
            if (User.IsInRole("Super Admin"))
            {
                return RedirectToAction("Index", "UserAdmin");
            }
            else if (User.IsInRole("HR"))
            {
                return RedirectToAction("Index", "CompanyHR");
            }
            else
            {
                return RedirectToAction("Home");
            }
        }

        public async Task<ActionResult> Home()
        {
            string loggedInUserId = GetUserId(); 
            ViewBag.UserId = loggedInUserId;
            var user = await _gEmployeeService.GetByUserIdAsync(e => e.UserId == loggedInUserId);
            string name = user?.FullName;
            ViewBag.Name = !string.IsNullOrEmpty(name) ? name : "Not Exists";

            return View();
        }
        private string GetUserId()
        {
            try
            {
                
                return Request.Cookies["userId"];
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error in method: *GetUserId()* Message: *{ex.Message}*";
                SendException(exceptionMessage);
                return null;
            }
        }
        [HttpPost]
        public ActionResult PersonalDetails(EmployeeMetadata employee, EmployeeImg eImg)
        {
            string userId = GetUserId();

            //if (ModelState.IsValid)
            //{
            try
            {
                if (eImg.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(eImg.ImageFile?.FileName);
                    string fileExtension = Path.GetExtension(eImg.ImageFile?.FileName);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        eImg.ImageFile?.CopyToAsync(ms);
                        employee.Photo = ms.GetBuffer();
                    }
                }

                var employeeDb = _eService?.GetById(userId);

                if (employeeDb != null)
                {
                    //db.Employees.Attach(employee);
                    //db.Entry(employee).State = EntityState.Modified;
                    employee.CompanyId = employeeDb.CompanyId;
                    employee.OfficialEmployeeId = employeeDb.OfficialEmployeeId;
                    employee.OfficialEmailId = employeeDb.OfficialEmailId;
                    employee.OfficialEmailIdPassword = employeeDb.OfficialEmailIdPassword;
                    _eService.Update(employee);

                }
                else
                {
                    employee.UserId = userId;
                    employee.AddedDate = DateTime.Now;
                    _eService.Add(employee);
                }

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
                //}
            }
            catch (Exception ex)
            {
                ViewBag.BloodGroup = GetBloodGroups(employee.BloodGroup);
                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = _eService.GetById(uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *PersonalDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                return RedirectToAction("Home", "EmployeeV1");
            }
        }
        List<SelectListItem> GetBloodGroups(string bloodGroup)
        {
            var bloodGroups = new List<SelectListItem>() {
                new SelectListItem(){ Text = "Not Applicable", Value="Not Applicable"},
                new SelectListItem(){ Text = "A+", Value="A+"},
                new SelectListItem(){ Text = "A-", Value="A-"},
                new SelectListItem(){ Text = "B+", Value="B+", Selected=true},
                new SelectListItem(){ Text = "B-", Value="B-"},
                new SelectListItem(){ Text = "AB+", Value="AB+"},
                new SelectListItem(){ Text = "AB-", Value="AB-"},
                new SelectListItem(){ Text = "O+", Value="O+"},
                new SelectListItem(){ Text = "O-", Value="O-"}
            };

            if (!string.IsNullOrEmpty(bloodGroup))
            {
                bloodGroups.ForEach(b =>
                {
                    if (b.Text.Equals(bloodGroup))
                        b.Selected = true;
                });
            }

            return bloodGroups;
        }

        [HttpPost]
        public ActionResult EducationalDetails(IFormCollection employeeEducations)
        {
            try
            {
                string userId = GetUserId();

                string[] educationIds = employeeEducations["EducationId"].ToString()?.Split(',');
                string[] typeIds = employeeEducations["EducationTypeId"].ToString()?.Split(',');
                string[] passoutYears = employeeEducations["PassoutYear"].ToString()?.Split(',');
                string[] specializations = employeeEducations["Specialization"].ToString()?.Split(',');
                string[] percentages = employeeEducations["Percentage"].ToString()?.Split(',');
                string[] collegeNames = employeeEducations["CollegeName"].ToString()?.Split(',');

                List<EmployeeEducationMetadata> educations =
                    new List<EmployeeEducationMetadata>();

                for (int i = 0; i < typeIds?.Length; i++)
                {
                    int eType, pYear;
                    decimal per;
                    int.TryParse(typeIds[i], out eType);
                    int.TryParse(passoutYears[i], out pYear);
                    decimal.TryParse(percentages[i], out per);

                    EmployeeEducationMetadata edu = new EmployeeEducationMetadata()
                    {
                        UserId = userId,
                        EducationTypeId = eType,
                        Specialization = specializations[i],
                        PassoutYear = pYear,
                        Percentage = per,
                        CollegeName = collegeNames[i]
                    };

                    int educationId = 0;
                    if (educationIds != null && educationIds.Length > 0 && i < educationIds.Length)
                    {
                        int.TryParse(educationIds[i], out educationId);
                    }
                    var educationDb = _eService.GetEmployeeEducationById(educationId);

                    if (educationDb != null)
                    {
                        edu.EducationId = educationDb.EducationId;
                        _eService.UpdateEmployeeEducation(educationDb, edu);
                    }
                    else
                    {
                        _eService.AddEmployeeEducation(edu);
                    }

                    

                    educations.Add(edu);
                }

                ViewBag.EducationTypeIdList = new SelectList
                    (_eService.GetEducationType(), "EducationTypeId", "EducationName");

                //db.EmployeeEducations.AddRange(educations);
                //db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                ViewBag.EducationTypeIdList = new SelectList
                (_eService.GetEducationType(), "EducationTypeId", "EducationName");

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = _eService.GetById(uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *EducationalDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        [HttpGet]
        public ActionResult DeleteEducation(int educationId)
        {
            _eService.DeleteEmployeeEducation(educationId);
            return RedirectToAction("Home", "EmployeeV1");
        }

        public PartialViewResult EducationalNewForm()
        {
            ViewBag.EducationTypeIdList = new SelectList
                (_eService.GetEducationType(), "EducationTypeId", "EducationName");

            return PartialView("_newEducationalDetailsForm");
        }

        public PartialViewResult BankNewForm()
        {
            return PartialView("_newBankForm");
        }
        [HttpPost]
        public async Task<ActionResult> BankDetailsAsync(IFormCollection bankDetails)
        {
            try
            {
                string userId = GetUserId();

                string[] accountIds = bankDetails["AccountId"].ToString()?.Split(',');
                string[] bankNames = bankDetails["BankName"].ToString()?.Split(',');
                string[] branchNames = bankDetails["BranchName"].ToString()?.Split(',');
                string[] accountNumbers = bankDetails["AccountNumber"].ToString()?.Split(',');
                string[] iFSCCodes = bankDetails["IFSCCode"].ToString()?.Split(',');

                List<EmployeeBankAccountMetadata> accounts =
                    new List<EmployeeBankAccountMetadata>();

                for (int i = 0; i < bankNames.Length; i++)
                {
                    EmployeeBankAccountMetadata account = new EmployeeBankAccountMetadata()
                    {
                        UserId = userId,
                        BankName = bankNames[i],
                        BranchName = branchNames[i],
                        AccountNumber = accountNumbers[i],
                        IFSCCode = iFSCCodes[i],
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    int accountId = 0;
                    if (accountIds != null && accountIds.Length > 0 && i < accountIds.Length)
                    {
                        int.TryParse(accountIds[i], out accountId);
                    }

                    EmployeeBankAccountMetadata bankDb = await _gEBankAccountService.GetByIdAsync(accountId);

                    if (bankDb != null)
                    {
                        account.AccountId = bankDb.AccountId;
                        await _gEBankAccountService.UpdateAsync(bankDb.AccountId, account);
                    }
                    else
                    {
                        await _gEBankAccountService.AddAsync(account);
                    }


                    accounts.Add(account);
                }

                // db.EmployeeBankAccounts.AddRange(accounts);                

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                EmployeeMetadata employee = await _gEmployeeService.GetByUserIdAsync(e => e.UserId == uId);
                string name = employee.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *BankDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }
        [HttpGet]
        public async Task<ActionResult> DeleteBankDetails(int accountId)
        {
            //EmployeeBankAccountMetadata account = await _gEBankAccountService.GetByIdAsync(accountId);
            await _gEBankAccountService.DeleteAsync(accountId);
            //db.SaveChanges();
            return RedirectToAction("Home", "EmployeeV1");
        }
        public async Task<PartialViewResult> ExperienceNewForm()
        {
            string userId = GetUserId();
            IEnumerable<EmployeeBankAccountMetadata> accounts = await _gEBankAccountService.GetByUserIdAsync(e => e.UserId ==  userId, true);
            ViewBag.AccountIdList = new SelectList
                (accounts?.ToList(), "AccountId", "BankName");

            return PartialView("_newExperienceForm");
        }
        [HttpPost]
        public async Task<ActionResult> ExperienceDetails(IFormCollection experienceDetails)
        {
            string userId = GetUserId();
            try
            {
                string[] experienceIds = experienceDetails["ExperienceId"].ToString()?.Split(',');
                string[] companyNames = experienceDetails["CompanyName"].ToString()?.Split(',');
                string[] joiningDates = experienceDetails["JoiningDate"].ToString()?.Split(',');
                string[] currentCTCs = experienceDetails["CurrentCTC"].ToString()?.Split(',');
                string[] relivingDates = experienceDetails["RelivingDate"].ToString()?.Split(',');
                List<string> isPFOpteds =
                    experienceDetails["item.IsPFOpted"].ToString()?.Split(',')?.ToList();

                if (isPFOpteds == null)
                {
                    isPFOpteds = new List<string>();
                }

                isPFOpteds.AddRange(experienceDetails["IsPFOpted"].ToString()?.Split(',')?.ToList());

                List<string> isAllDocumentsAvailables =
                    experienceDetails["item.IsAllDocumentsAvailable"].ToString()?.Split(',')?.ToList();

                if (isAllDocumentsAvailables == null)
                {
                    isAllDocumentsAvailables = new List<string>();
                }

                isAllDocumentsAvailables.AddRange(
                    experienceDetails["IsAllDocumentsAvailable"].ToString()?.Split(',')?.ToList());

                string[] accountIds = experienceDetails["AccountId"].ToString()?.Split(',');

                List<EmployeeExperieceMetadata> experiences =
                    new List<EmployeeExperieceMetadata>();

                for (int i = 0; i < companyNames.Length; i++)
                {
                    DateTime doj, relDate;
                    DateTime.TryParse(joiningDates[i], out doj);
                    decimal cCTC;
                    decimal.TryParse(currentCTCs[i], out cCTC);
                    DateTime.TryParse(relivingDates[i], out relDate);
                    bool isPF = false, isDoc = false;
                    if (!(string.IsNullOrEmpty(isPFOpteds[i])))
                    {
                        bool.TryParse(isPFOpteds[i], out isPF);
                    }
                    if (!(string.IsNullOrEmpty(isAllDocumentsAvailables[i])))
                    {
                        bool.TryParse(isAllDocumentsAvailables[i], out isDoc);
                    }

                    int accId;
                    int.TryParse(accountIds[i], out accId);

                    EmployeeExperieceMetadata account = new EmployeeExperieceMetadata()
                    {
                        UserId = userId,
                        CompanyName = companyNames[i],
                        JoiningDate = doj,
                        CurrentCTC = cCTC,
                        RelivingDate = relDate,
                        IsPFOpted = isPF,
                        IsAllDocumentsAvailable = isDoc,
                        AccountId = accId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    int experienceId = 0;
                    if (experienceIds != null && experienceIds.Length > 0 && i < experienceIds.Length)
                    {
                        int.TryParse(experienceIds[i], out experienceId);
                    }

                    EmployeeExperieceMetadata experienceDb = await _gEExperienceService.GetByIdAsync(experienceId);

                    if (experienceDb != null)
                    {
                        account.ExperienceId = experienceDb.ExperienceId;
                        await _gEExperienceService.UpdateAsync(experienceDb.ExperienceId, account);
                    }
                    else
                    {
                        await _gEExperienceService.AddAsync(account);
                    }

                    experiences.Add(account);
                }

                // db.EmployeeExperieces.AddRange(experiences);


                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                IEnumerable<EmployeeBankAccountMetadata> bankAccount = await _gEBankAccountService.GetByUserIdAsync(e => e.UserId == userId, true);
                ViewBag.AccountIdList = new SelectList
                    (bankAccount?.ToList(), "AccountId", "BankName");

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                EmployeeMetadata employee = await _gEmployeeService.GetByUserIdAsync(e => e.UserId == uId);
                string name = employee?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *ExperienceDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }
        [HttpGet]
        public async Task<ActionResult> DeleteExperience(int experienceId)
        {
            //EmployeeExperiece experience = db.EmployeeExperieces.Find(experienceId);
            await _gEExperienceService.DeleteAsync(experienceId);
            //db.SaveChanges();
            return RedirectToAction("Home", "EmployeeV1");
        }

        [HttpPost]
        public async Task<ActionResult> ITExperienceDetails(EmployeeITExperienceMetadata iTExperience)
        {
            try
            {
                string userId = GetUserId();

                iTExperience.CreatedDate = DateTime.Now;
                iTExperience.UserId = userId;
                iTExperience.ModifiedDate = DateTime.Now;

                EmployeeITExperienceMetadata experience = await _gEITExperienceservice.GetByIdAsync(iTExperience.ExperienceId);
                if (experience != null)
                {
                    await _gEITExperienceservice.UpdateAsync(experience.ExperienceId, iTExperience);
                }
                else
                {
                    _gEITExperienceservice.AddAsync(iTExperience);
                }


                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                IEnumerable<BatchMetadata> batches = await _gBatchService.GetAllAsync();
                IEnumerable<TeamMetadata> teams = await _gTeamService.GetAllAsync();
                ViewBag.BatchIdList = new SelectList
                    (batches?.ToList(), "BatchId", "BatchName");

                ViewBag.TeamIdList = new SelectList
                    (teams?.ToList(), "TeamId", "TeamName");

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                EmployeeMetadata employee = await _gEmployeeService.GetByUserIdAsync(e => e.UserId == uId);
                string name = employee?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *ITExperienceDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Resignation(EmployeeResignationMetadata resignation)
        {
            try
            {
                resignation.UserId = GetUserId();

                EmployeeResignationMetadata resignationDb = await _gEResignationService.GetByIdAsync(resignation.Id);

                if (resignationDb != null)
                {
                    await _gEResignationService.UpdateAsync(resignationDb.Id, resignation);
                }
                else
                {
                    await _gEResignationService.AddAsync(resignation);
                }


                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                EmployeeMetadata employee = await _gEmployeeService.GetByUserIdAsync(e => e.UserId == uId);
                string name = employee?.FullName;
                string exceptionMessage = $"*{name}* Resigned. Resignation Date: *{resignation.ResignationDate}* Last Working Day: *{resignation.LastWorkingDay}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                EmployeeMetadata employee = await _gEmployeeService.GetByUserIdAsync(e => e.UserId == uId);
                string name = employee?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *Resignation()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        [HttpGet]
        public ActionResult ResetUserNameOrPassword()
        {
            return View();
        }


        void SendException(string exception)
        {
            try
            {
                //try to change this code

                //WhatsAppRequest notification = new WhatsAppRequest()
                //{
                //    Phone = ConfigConstants.WAToNumbers,
                //    Body = exception,
                //    Type = MessageType.text
                //};

                //WhatsAppNotification.Send(notification);
            }
            catch { }
        }

    }
}
