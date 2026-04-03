using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public interface IUserRepository: IRepository<LocalUser>
    {
        bool IsUniqueUser(string username);

        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
