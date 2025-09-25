using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EM.Models.Entities
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class EmployeeImg
    {
        public IFormFile ImageFile { get; set; }
    }

    public class EmployeeMetadata
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        [DisplayName("Full Name as on PAN *")]
        public string FullName { get; set; }

        [DisplayName("Upload Photo *")]
        public byte[] Photo { get; set; }

        [Required]
        [DisplayName("Permanent Address *")]
        public string PermanentAddress { get; set; }

        [Required]
        [DisplayName("Current Address *")]
        public string CurrentAddress { get; set; }

        [Required]
        [RegularExpression("[A-Z]{5}\\d{4}[A-Z]{1}")]
        [DisplayName("PAN Number *")]
        public string PAN { get; set; }

        [Required]
        [RegularExpression("[0-9]{10}")]
        [DisplayName("Mobile Number *")]
        public string Mobile { get; set; }

        [RegularExpression("[0-9]{10}")]
        [DisplayName("Alternate Mobile Number")]
        public string AlternateMobile { get; set; }

        [Required]
        [RegularExpression("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)")]
        [DisplayName("Email Address *")]
        public string EmailId { get; set; }

        [RegularExpression("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)")]
        [DisplayName("Alternate Email Address")]
        public string AlternateEmailId { get; set; }

        [Required]
        [DisplayName("Blood Group *")]
        public string BloodGroup { get; set; }

        //[Required]
        [DisplayName("Highest Qualification *")]
        public string HighestQualification { get; set; }

        //[Required]
        [DisplayName("Highest Qualification Passout Month & Year *")]
        public string HighestQualificationPassoutMonthYear { get; set; }

        //[Required]
        [DisplayName("Technology - Skills Set *")]
        public string Technology { get; set; }

        //[Required]
        [DisplayName("Offer Date *")]
        public DateOnly? OfferDate { get; set; }

        //[Required]
        [DisplayName("Joining Date *")]
        public DateOnly? JoiningDate { get; set; }

        //[Required]
        [DisplayName("Offer Designation *")]
        public string OfferDesignation { get; set; }

        //[Required]
        [DisplayName("Current Designation *")]
        public string CurrentDesignation { get; set; }

        //[Required]
        [DisplayName("Offer Salary *")]
        public decimal? OfferSalary { get; set; }

        //[Required]
        [DisplayName("Current Salary *")]
        public decimal? CurrentSalary { get; set; }

        //[Required]
        [DisplayName("Last Hike Month & Year *")]
        public string LastHikeMonthYear { get; set; }

        [DisplayName("Resignation Date")]
        public DateOnly? ResignationDate { get; set; }

        [DisplayName("Reliving Date")]
        public DateOnly? RelivingDate { get; set; }

        [DisplayName("Official Employee Id")]
        public string OfficialEmployeeId { get; set; }

        [DisplayName("Official Email Id")]
        public string OfficialEmailId { get; set; }

        [DisplayName("Official Email Password")]
        public string OfficialEmailIdPassword { get; set; }

        //[Required]
        public int? CompanyId { get; set; }
        public string UAN { get; set; }

        public DateTime? AddedDate { get; set; }

        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly? DateOfBirth { get; set; }

        [Required]
        [RegularExpression("[0-9]{12}")]
        [DisplayName("Adhaar Number *")]
        public string Adhaar { get; set; }
    }
}