using System.ComponentModel.DataAnnotations;

namespace RegistrationApllication.Modal
{
    public class RegistrationModelClass
    {
        [Key]

        public int CandidateId { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string Experience { get; set; }
        public string Fresher { get; set; }
        public string SkillSet { get; set; }
        public string EducationDetails { get; set; }
        public string CurrentCtc { get; set; }
        public string ExpectedCtc { get; set; }
        public string AttachmentIds { get; set; }
        public string Availabilty { get; set; }
        public string PassPort { get; set; }
        public string IdentityCardNumber { get; set; }
        public string CountryResiding { get; set; }
        public string Citizenship { get; set; }
        public string CurrentRole { get; set; }
        public string PositionApplied { get; set; }
        public string CurrentCity { get; set; }
        public string ApplicantStatus { get; set; }
        public string MessageField { get; set; }
        public bool IsArchived { get; set; } = false;
        public string Remarks { get; set; }
        public string status { get; set; }
    }
}
