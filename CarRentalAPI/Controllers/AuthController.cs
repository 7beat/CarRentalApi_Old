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
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto loginRequest)
        {
            var userResult = await repository.RegisterUserAsync(loginRequest);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }

        //[HttpPost]
        //[Route("login")]
        //public IActionResult SignUp(Models.DTO.LoginRequest loginRequest)
        //{

        //    return Ok("Logged");
        //}
    }
}
