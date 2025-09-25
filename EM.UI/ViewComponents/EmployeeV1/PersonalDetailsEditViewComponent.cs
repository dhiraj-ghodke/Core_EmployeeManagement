using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EM.UI.ViewComponents.EmployeeV1
{
    public class PersonalDetailsEditViewComponent : ViewComponent
    {
        private readonly IEmployeeService _eService;
        public PersonalDetailsEditViewComponent(IEmployeeService eService)
        {
            _eService = eService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = Request.Cookies["userId"];
            var employee = _eService.GetById(userId);

            ViewBag.BloodGroup = GetBloodGroups(employee?.BloodGroup);
            if (employee != null)
                employee.DateOfBirth = employee?.DateOfBirth;
            return View(employee);
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
    }
}
