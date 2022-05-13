using System.ComponentModel.DataAnnotations;

namespace RegistrationApllication.Modal
{

    public class UserModelClass
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
