using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EM.Models
{
    [MetadataType(typeof(EmployeeITExperienceMetadata))]
    public partial class EmployeeITExperience
    {
    }

    public class EmployeeITExperienceMetadata
    {
        public int ExperienceId { get; set; }
        public string UserId { get; set; }

        [DisplayName("Batch No")]
        public Nullable<int> BatchId { get; set; }

        [DisplayName("Team No")]
        public Nullable<int> TeamId { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [DisplayName("Project Technologies")]
        public string ProjectTechnologies { get; set; }

        [DisplayName("Project Description")]
        public string ProjectDescription { get; set; }

        [DisplayName("Joining Date")]
        [Required]
        public Nullable<System.DateTime> JoiningDate { get; set; }

        [DisplayName("Joining Salary")]
        [Required]
        public Nullable<decimal> JoiningSalary { get; set; }

        [DisplayName("Joining Designation")]
        public string JoiningDesignation { get; set; }

        [DisplayName("Current Salary")]
        [Required]
        public Nullable<decimal> CurrentSalary { get; set; }

        [DisplayName("Current Designation")]
        public string CurrentDesignation { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

        [DisplayName("Bank Account For Salary")]
        public Nullable<int> BankAccountId { get; set; }

        
    }
}