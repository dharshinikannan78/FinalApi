using System.ComponentModel.DataAnnotations;

namespace RegistrationApllication.Modal
{
    public class FileAttachmentModelClass
    {
        [Key]

        public int AttachmentId { get; set; }
        [MaxLength(100)]
        public string AttachmentName { get; set; }
        [MaxLength(100)]
        public string AttachmentType { get; set; }
        [MaxLength(500)]
        public string AttachmentPath { get; set; }
    }
}
