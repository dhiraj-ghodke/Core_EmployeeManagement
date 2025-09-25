using EM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    public interface ICompanyDocumentService
    {
        Task<IEnumerable<CompanyDocumentMetadata>> GetCompanyDocumentWithAllDetails();
    }
}
