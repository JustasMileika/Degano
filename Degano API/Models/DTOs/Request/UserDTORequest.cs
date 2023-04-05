using System.ComponentModel.DataAnnotations;

namespace Degano_API.Models.DTOs.Request
{
    public class UserDTORequest
    {
        public UserDTORequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public UserDTORequest()
        {

        }

        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
