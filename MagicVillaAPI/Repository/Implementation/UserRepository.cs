using AutoMapper;
using MagicVillaAPI.DBContext;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVillaAPI.Repository.Implementation
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDBContext _db;
		private readonly IMapper _mapper;
		private readonly string _secretKey;
		public UserRepository(ApplicationDBContext db, IMapper mapper, IConfiguration configuration)
		{
			_db = db;
			_mapper = mapper;
			_secretKey = configuration.GetValue<string>("ApiSettings:Secret") ?? "";
			
		}

		public async Task<bool> IsUniqueUser(string username) => await _db.LocalUsers.FirstOrDefaultAsync(u => u.Name == username) == null;

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = await _db.LocalUsers.FirstOrDefaultAsync(u => u.UserName == loginRequestDTO.UserName && u.Password == loginRequestDTO.Password);
			if (user == null)
			{
				return new LoginResponseDTO
				{
					User = null,
					Token = ""
				}; ;
			}
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Role, user.Role),
					new Claim("issueTime", DateTime.Now.ToString())
				}),
				Expires = DateTime.UtcNow.AddHours(.9),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new LoginResponseDTO
			{
				User = user,
				Token = tokenHandler.WriteToken(token)
			};
			
		}

		public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
		{
			LocalUser user = _mapper.Map<LocalUser>(registrationRequestDTO);
			_db.LocalUsers.Add(user);
			await _db.SaveChangesAsync();
			user.Password = "";
			return user;
		}
	}
}
