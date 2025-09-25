using AutoMapper;
using EM.Models;
using EM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Services
{
    public class CompanyDocumentService : ICompanyDocumentService
    {
        private readonly ICompanyDocumentRepository _companyDocumentRepository;
        private readonly IMapper _mapper;

        public CompanyDocumentService(ICompanyDocumentRepository companyDocumentRepository, IMapper mapper)
        {
            _companyDocumentRepository = companyDocumentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDocumentMetadata>> GetCompanyDocumentWithAllDetails()
        {
            IEnumerable<EM.Core.Models.Entities.CompanyDocument> companyDocuments = await _companyDocumentRepository.GetCompanyDocumentWithAllDetails();
            

            return _mapper.Map<IEnumerable<CompanyDocumentMetadata>>(companyDocuments);
        }
    }
}
