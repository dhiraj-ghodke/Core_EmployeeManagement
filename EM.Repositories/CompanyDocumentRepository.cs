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
    public class CompanyDocumentRepository : ICompanyDocumentRepository
    {
        private readonly EmployeeDBContext _dbContext;

        public CompanyDocumentRepository(EmployeeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CompanyDocument>> GetCompanyDocumentWithAllDetails()
        {
            IEnumerable<CompanyDocument> companyDocuments = await _dbContext.CompanyDocuments.Include(c => c.CreatedByNavigation).Include(c1 => c1.Company).Include(c2 => c2.CompanyDocumentsType).ToListAsync();
            return companyDocuments;
        }
    }
}
