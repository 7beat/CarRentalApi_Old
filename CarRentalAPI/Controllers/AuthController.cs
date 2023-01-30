using CarRentalAPI.Models.DTO;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticationRepository repository;
        public AuthController(IUserAuthenticationRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto registerRequest)
        {
            var userResult = await repository.RegisterUserAsync(registerRequest);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] Models.DTO.UserLoginDto loginRequest)
        {
            var result = await repository.ValidateUserAsync(loginRequest);
            return Ok(result);
        }
    }
}
