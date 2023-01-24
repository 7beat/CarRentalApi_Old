using AutoMapper;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(Repositories.IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersDomain = await userRepository.GetAllAsync();

            //Convert to DTO
            var usersDTO = mapper.Map<List<Models.DTO.User>>(usersDomain);

            return Ok(usersDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDomain = await userRepository.GetByIdAsync(id);

            var userDTO = mapper.Map<Models.DTO.User>(userDomain);

            return Ok(userDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] Models.DTO.AddUserRequest user)
        {
            if (!await ValidateAddUserRequestAsync(user))
                return BadRequest(ModelState);

            var userDomain = new Models.Domain.User
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDay = user.BirthDay
            };

            userDomain = await userRepository.AddAsync(userDomain);

            var userDTO = mapper.Map<Models.DTO.User>(userDomain);

            return Ok(userDTO);
        }

        private async Task<bool> ValidateAddUserRequestAsync(Models.DTO.AddUserRequest user)
        {
            if (!await userRepository.IsEmailUnique(user.Email))
            {
                ModelState.AddModelError(nameof(user), $"{nameof(user.Email)} is already taken!");
                return false;
            }

            if (!await userRepository.IsUsernameUnique(user.Username))
            {
                ModelState.AddModelError(nameof(user), $"{nameof(user.Username)} is already taken!");
                return false;
            }

            return true;
        }
    }
}
