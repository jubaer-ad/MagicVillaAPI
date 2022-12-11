using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;

namespace MagicVillaAPI.Repository.IRepository
{
	public interface IUserRepository
	{
		Task<bool> IsUniqueUser(string username);
		Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
		Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO);
	}
}
