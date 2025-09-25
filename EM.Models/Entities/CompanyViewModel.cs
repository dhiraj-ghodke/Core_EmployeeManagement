using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EM.Models
{
    [MetadataType(typeof(CompanyMetadata))]
    public partial class Company
    {

    }

    public class CompanyMetadata
    {
        public int CompanyId { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        [Required]
        [DisplayName("Manager Employee Id")]
        public string ManagerId { get; set; }

        [Required]
        [DisplayName("Manager Name")]
        public string ManagerName { get; set; }

        [Required]
        [DisplayName("Manager Mobile Number")]
        public string ManagerMobile { get; set; }

        [Required]
        [DisplayName("Manager Official Email Id")]
        public string ManagerOfficialEmail { get; set; }

        [Required]
        [DisplayName("HR Employee Id")]
        public string HRId { get; set; }

        [Required]
        [DisplayName("HR Name")]
        public string HRName { get; set; }

        [Required]
        [DisplayName("HR Mobile Number")]
        public string HRMobile { get; set; }

        [Required]
        [DisplayName("HR Official Email Id")]
        public string HROfficialEmail { get; set; }
    }
}