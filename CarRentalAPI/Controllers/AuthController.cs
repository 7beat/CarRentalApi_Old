using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost]
        [Route("login")]
        public IActionResult Login(Models.DTO.LoginRequest loginRequest)
        {

            return Ok("Logged");
        }

        //[HttpPost]
        //[Route("login")]
        //public IActionResult SignUp(Models.DTO.LoginRequest loginRequest)
        //{

        //    return Ok("Logged");
        //}
    }
}
