using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.User;
using OnlineShoppingApp.Business.Operations.User.Dtos;
using OnlineShoppingApp.WebApi.Jwt;
using OnlineShoppingApp.WebApi.Models;
using LoginRequest = OnlineShoppingApp.WebApi.Models.LoginRequest;
using RegisterRequest = OnlineShoppingApp.WebApi.Models.RegisterRequest;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
            {
                return Ok();
            }

            return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.LoginUser(new LoginUserDto
                { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                PhoneNumber = user.PhoneNumber,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!),
            });

            return Ok(new LoginResponse
            {
                Message = "Login Succeed",
                Token = token
            });
        }
    }
}