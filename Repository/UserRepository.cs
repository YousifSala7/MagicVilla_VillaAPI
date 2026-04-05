using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Dto;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository: Repository<LocalUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration): base(db)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = dbSet.FirstOrDefault(x => x.UserName == username);

            return user == null;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
				Password = BCrypt.Net.BCrypt.HashPassword(registrationRequestDTO.Password),
				Name = registrationRequestDTO.Name,
                Role = registrationRequestDTO.Role
            };

            await CreateAsync(user);

            //user.Password = "";
            var userdto = _mapper.Map<UserDTO>(user);
            return userdto;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await dbSet.FirstOrDefaultAsync(u => u.UserName == loginRequestDTO.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.Password))
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
				Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _configuration.GetValue<string>("ApiSettings:Issuer"),
                Audience = _configuration.GetValue<string>("ApiSettings:Audience"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
            };
            return loginResponseDTO;
            }
        }
    }
