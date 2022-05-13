using System.ComponentModel.DataAnnotations;

namespace RegistrationApllication.Modal
{
    public class TokenRequest
    {
        [Key]
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
        public string Token { get; set; }
        public int TokenId { get; set; }

        
        
        
    }
}
