using EM.Models.Entities;
using EM.Models;

namespace EM.Services
{
    public interface IEmployeeService
    {
        EmployeeMetadata GetById(string userId);
        
        void Update(EmployeeMetadata employee);
        void Add(EmployeeMetadata employee);
        List<EmployeeEducationMetadata> GetEducation(string userId);
        IEnumerable<EmployeeEducationTypeViewModel> GetEducationType();
        EmployeeEducationMetadata GetEmployeeEducationById(int? educationId);
        void UpdateEmployeeEducation(EmployeeEducationMetadata educationDb, EmployeeEducationMetadata edu);
        void AddEmployeeEducation(EmployeeEducationMetadata edu);
        void DeleteEmployeeEducation(int educationId);
    }
}
