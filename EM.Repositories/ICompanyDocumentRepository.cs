using EM.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Repositories
{
    public interface ICompanyDocumentRepository
    {
        Task<IEnumerable<CompanyDocument>> GetCompanyDocumentWithAllDetails();
    }
}
