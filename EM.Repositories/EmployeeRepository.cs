using EM.Core.Models;
using EM.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _dbContext;

        public EmployeeRepository(EmployeeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Employee GetById(string userId)
        {
            Employee employee = new Employee();
            if(!String.IsNullOrEmpty(userId))
            {
                employee = _dbContext.Employees.AsNoTracking().FirstOrDefault(e => e.UserId == userId);
                return employee;
            }
            return employee;
        }
        
        
        public void Update(Employee employee)
        {
            if(_dbContext.Employees.Any(u => u.UserId == employee.UserId))
            {
                _dbContext.Entry(employee).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return;
            }
            return;
        }
        public void Add(Employee employee)
        {
            _dbContext.Add(employee);
            _dbContext.SaveChanges();
            return;
        }

        public List<EmployeeEducation> GetEducation(string userId)
        {
            return _dbContext.EmployeeEducations.Where(e => e.UserId == userId).ToList();
        }

        public List<EmployeeEducation> GetEmployeeEducation(string userId)
        {
            return _dbContext.EmployeeEducations.Where(e => e.UserId.Equals(userId)).ToList();
        }

        public IEnumerable<EmployeeEducationType> GetEmployeeEducationType()
        {
            
            return _dbContext.EmployeeEducationTypes;
        }

        public EmployeeEducation GetEmployeeEducationById(int? educationId)
        {
            return _dbContext.EmployeeEducations.AsNoTracking().FirstOrDefault(e => e.EducationId == educationId);
        }

        public void UpdateEmployeeEducation(EmployeeEducation educationDb, EmployeeEducation edu)
        {
            _dbContext.Entry(educationDb).CurrentValues.SetValues(edu);
            _dbContext.Entry(educationDb).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void AddEmployeeEducation(EmployeeEducation edu)
        {
            _dbContext.EmployeeEducations.Add(edu);
            _dbContext.SaveChanges();
        }

        public void DeleteEmployeeEducation(int educationId)
        {
            EmployeeEducation employeeEducation = GetEmployeeEducationById(educationId);
            _dbContext.Remove(employeeEducation);
            _dbContext.SaveChanges();
        }
    }
}
