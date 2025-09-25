using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EM.Models
{
    [MetadataType(typeof(CompanyDocumentMetadata))]
    public partial class CompanyDocument
    {
        [DisplayName("Upload File")]
        public IFormFile DocumentFile { get; set; }
    }

    public class CompanyDocumentMetadata
    {
        public int Id { get; set; }

        [DisplayName("Document Type")]
        public string DocumentName { get; set; }
        public byte[]? DocumentPath { get; set; }
        [DisplayName("Upload File")]
        public IFormFile DocumentFile { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<int> CompanyId { get; set; }

        public string? DocumentType { get; set; }
        public int? CompanyDocumentsTypeId { get; set; }
        [DisplayName("Created By")]
        public UserModel? CreatedByNavigation { get; set; }
        public CompanyMetadata? Company {  get; set; }
        public CompanyDocumentsTypeMetadata? CompanyDocumentsType { get; set; }

    }
}