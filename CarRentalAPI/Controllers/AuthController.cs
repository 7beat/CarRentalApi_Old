using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Encodings.Web;

namespace CarRentalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly IEmailSender emailSender;

        public AuthController(IRepositoryManager repository, IEmailSender emailSender)
        {
            this.repository = repository;
            this.emailSender = emailSender;
        }

        [HttpPost]
        [Route("Register")]
        [SwaggerOperation(Summary = "Create new Account", Description = "User needs to provide unique Email and Username")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto registerRequest)
        {
            var userResult = await repository.UserAuthentication.RegisterUserAsync(registerRequest);

#if true // Email confirmation

            if (userResult.Succeeded)
            {
                var tokenData = await repository.UserAuthentication.CreateEmailCredentials();

                var callbackUrl = Url.Action("ConfirmEmail", "Auth",
                    new { userId = tokenData.userId, code = tokenData.token }, Request.Scheme);

                await emailSender.SendEmailAsync(registerRequest.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return StatusCode(StatusCodes.Status201Created, "Confirmation email was sent!");
            }

#endif
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

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(int userId, string code)
        {
            var user = await repository.Users.GetByIdAsync(userId);
            if (user is null)
                return BadRequest("Invalid user");
            
            var result = await repository.UserAuthentication.ConfirmUserEmail(user, code);

            if (result.Succeeded)
            {
                return Ok("Email confirmed");
            }
            else
            {
                return BadRequest("Invalid confirmation code");
            }
        }
    }
}
