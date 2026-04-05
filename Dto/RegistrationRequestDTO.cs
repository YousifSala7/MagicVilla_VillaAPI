using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Dto
{
    public class RegistrationRequestDTO
    {
        [Required(ErrorMessage = "Username required!")]
        [MinLength(3, ErrorMessage = "The username must be at least 3 characters long.")]
		public string UserName { get; set; }

        [Required(ErrorMessage = "Name required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password required!")]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
