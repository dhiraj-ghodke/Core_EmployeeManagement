using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EM.Models
{
    public partial class CompanyDocumentsTypeMetadata
    {
        public int DocumentTypeId { get; set; }

        
        public string DocumentTypeName { get; set; }

        public virtual ICollection<CompanyDocumentMetadata> CompanyDocuments { get; set; } = new List<CompanyDocumentMetadata>();
    }
}