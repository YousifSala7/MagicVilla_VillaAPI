using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Dto
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username required!")]
        public string UserName { get; set; }

		[Required(ErrorMessage = "Password required!")]
		public string Password { get; set; }
    }
}
