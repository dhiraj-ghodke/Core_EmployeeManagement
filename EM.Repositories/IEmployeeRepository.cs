using EM.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(string id);
        
        void Update(Employee employee);
        void Add(Employee employee);

        List<EmployeeEducation> GetEmployeeEducation(string userId);
        IEnumerable<EmployeeEducationType> GetEmployeeEducationType();
        EmployeeEducation GetEmployeeEducationById(int? educationId);
        void UpdateEmployeeEducation(EmployeeEducation educationDb, EmployeeEducation edu);
        void AddEmployeeEducation(EmployeeEducation edu);
        void DeleteEmployeeEducation(int educationId);
    }
}
