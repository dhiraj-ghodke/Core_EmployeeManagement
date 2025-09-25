using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EM.Models
{
    [MetadataType(typeof(EmployeeExperieceMetadata))]
    public partial class EmployeeExperiece
    {
    }

    public class EmployeeExperieceMetadata
    {
        public int ExperienceId { get; set; }
        public string UserId { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Joining Date")]
        public Nullable<System.DateTime> JoiningDate { get; set; }

        [DisplayName("CTC")]
        public Nullable<decimal> CurrentCTC { get; set; }

        [DisplayName("Reliving Date")]
        public Nullable<System.DateTime> RelivingDate { get; set; }

        [DisplayName("Is PF Opted?")]
        public Nullable<bool> IsPFOpted { get; set; }

        [DisplayName("Are Documents Available?")]
        public Nullable<bool> IsAllDocumentsAvailable { get; set; }

        [DisplayName("Bank Account")]
        public Nullable<int> AccountId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

        
        public virtual EmployeeBankAccount EmployeeBankAccount { get; set; }
    }
}