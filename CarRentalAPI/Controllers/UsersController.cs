using AutoMapper;
using CarRentalAPI.Models.Domain;
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

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(int id,
            Models.DTO.UpdateUserRequest updateUserRequest)
        {
            if (!await ValidateUpdateUserRequestAsync(updateUserRequest))
                return BadRequest(ModelState);

            var userDomain = new Models.Domain.User
            {
                Username = updateUserRequest.Username,
                FirstName = updateUserRequest.FirstName,
                LastName = updateUserRequest.LastName,
                BirthDay = updateUserRequest.BirthDay
            };

            userDomain = await userRepository.UpdateAsync(id, userDomain);

            if (userDomain is null)
                return NotFound();

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

        private async Task<bool> ValidateUpdateUserRequestAsync(Models.DTO.UpdateUserRequest updateUser)
        {
            if (!await userRepository.IsUsernameUnique(updateUser.Username))
            {
                ModelState.AddModelError(nameof(updateUser), $"{nameof(updateUser.Username)} is already taken!");
                return false;
            }
            return true;
        }
    }
}
