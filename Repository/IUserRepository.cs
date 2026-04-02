using MagicVilla_VillaAPI.Dto;

namespace MagicVilla_VillaAPI.Repository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
