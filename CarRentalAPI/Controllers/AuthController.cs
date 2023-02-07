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
        //private readonly IUserAuthenticationRepository repository;
        private readonly IRepositoryManager repoManager;

        public AuthController(IUserAuthenticationRepository repository, IRepositoryManager repoManager)
        {
            //this.repository = repository;
            this.repoManager = repoManager;
        }

        [HttpPost]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto registerRequest)
        {
            //var userResult = await repository.RegisterUserAsync(registerRequest);
            var userResult = await repoManager.UserAuthentication.RegisterUserAsync(registerRequest);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(StatusCodes.Status201Created);
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Authenticate([FromForm] UserLoginDto loginRequest)
        //    => !await repository.ValidateUserAsync(loginRequest) ? Unauthorized() : Ok(new { Token = await repository.CreateTokenAsync() });


        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Authenticate([FromBody] UserLoginDto loginRequest)
        //{
        //    if (!await repository.ValidateUserAsync(loginRequest)) //await repoManager.UserAuthentication.ValidateUserAsync(loginRequest);
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        return Ok(new { Token = await repository.CreateTokenAsync() });
        //    }
        //}

        [HttpPost]
        [Route("login2")]
        public async Task<IActionResult> Authenticate2([FromBody] UserLoginDto loginRequest)
        {
            if (!await repoManager.UserAuthentication.ValidateUserAsync(loginRequest))
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new { Token = await repoManager.UserAuthentication.CreateTokenAsync() });
            }
        }
    }
}
