using AutoMapper;
using EM.Core.Models.Entities;
using EM.Models;
using EM.Models.Entities;
using EM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public EmployeeMetadata GetById(string userId)
        {
            Employee employee = _repo.GetById(userId);
            return _mapper.Map<EmployeeMetadata>(employee);
        }
        
        public void Update(EmployeeMetadata employee)
        {

            _repo.Update(_mapper.Map<Employee>(employee));
        }
        public void Add(EmployeeMetadata employee)
        {
            Employee e1 = _mapper.Map<Employee>(employee);
            _repo.Add(e1);
        }
        public List<EmployeeEducationMetadata> GetEducation(string userId)
        {
            return _mapper.Map<List<EmployeeEducationMetadata>>(_repo.GetEmployeeEducation(userId));    
        }

        public IEnumerable<EmployeeEducationTypeViewModel> GetEducationType()
        {
            return _mapper.Map<IEnumerable<EmployeeEducationTypeViewModel>>(_repo.GetEmployeeEducationType());
        }

        public EmployeeEducationMetadata GetEmployeeEducationById(int? educationId)
        {
            return _mapper.Map<EmployeeEducationMetadata>(_repo.GetEmployeeEducationById(educationId));
        }

        public void UpdateEmployeeEducation(EmployeeEducationMetadata educationDb, EmployeeEducationMetadata edu)
        {
            _repo.UpdateEmployeeEducation(_mapper.Map<Core.Models.Entities.EmployeeEducation>(educationDb), _mapper.Map<Core.Models.Entities.EmployeeEducation>(edu));
        }

        public void AddEmployeeEducation(EmployeeEducationMetadata edu)
        {
            _repo.AddEmployeeEducation(_mapper.Map<Core.Models.Entities.EmployeeEducation>(edu));
        }

        public void DeleteEmployeeEducation(int educationId)
        {
            _repo.DeleteEmployeeEducation(educationId);
        }
    }
}
