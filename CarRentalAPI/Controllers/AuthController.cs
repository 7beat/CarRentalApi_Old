using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
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
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto loginRequest)
            => !await repository.ValidateUserAsync(loginRequest) ? Unauthorized() : Ok(new { Token = await repository.CreateTokenAsync() });


        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Authenticate([FromBody] Models.DTO.UserLoginDto loginRequest)
        //{
        //    if (!await repository.ValidateUserAsync(loginRequest))
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        return Ok(new { Token = await repository.CreateTokenAsync() });
        //    }
        //}
    }
}
