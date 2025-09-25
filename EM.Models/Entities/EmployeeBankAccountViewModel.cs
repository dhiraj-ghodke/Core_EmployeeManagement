using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EM.Models
{
    [MetadataType(typeof(EmployeeBankAccountMetadata))]
    public partial class EmployeeBankAccount
    {
    }

    public class EmployeeBankAccountMetadata
    {
        public int AccountId { get; set; }
        public string UserId { get; set; }

        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [DisplayName("Branch Name")]
        public string BranchName { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("IFSC Code")]
        public string IFSCCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

        //public virtual AspNetUser AspNetUser { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<EmployeeExperiece> EmployeeExperieces { get; set; }
    }
}