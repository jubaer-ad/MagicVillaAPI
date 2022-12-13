using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;

namespace MagicVillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/UserAuth")]
    [ApiVersionNeutral]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepository.Login(loginRequestDTO);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.IsSucces = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Username or password is incorrect!");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            bool isUserUnique = await _userRepository.IsUniqueUser(registrationRequestDTO.UserName);
            if (!isUserUnique)
            {
                _response.IsSucces = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Username already is in DB!");
                return BadRequest(_response);
            }
            var user = await _userRepository.Register(registrationRequestDTO);
            if (user == null)
            {
                _response.IsSucces = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Error while registering!");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSucces = true;
            return Ok(_response);
        }
    }
}
