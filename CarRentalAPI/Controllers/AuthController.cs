using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarRentalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepositoryManager repository;

        public AuthController(IRepositoryManager repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        [SwaggerOperation(Summary = "Create new Account", Description = "User needs to provide unique Email and Username")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto registerRequest)
        {
            var userResult = await repository.UserAuthentication.RegisterUserAsync(registerRequest);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(StatusCodes.Status201Created);
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Authenticate([FromForm] UserLoginDto loginRequest)
        //    => !await repository.ValidateUserAsync(loginRequest) ? Unauthorized() : Ok(new { Token = await repository.CreateTokenAsync() });

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary = "Login to Account", 
            Description = "Provide correct Username and Password, Lockout is enabled 5 attempts will block account for 5 minutes!")]
        public async Task<IActionResult> Authenticate([FromForm] UserLoginDto loginRequest)
        {
            if (!await repository.UserAuthentication.ValidateUserAsync(loginRequest))
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new { Token = await repository.UserAuthentication.CreateTokenAsync() });
            }
        }
    }
}
