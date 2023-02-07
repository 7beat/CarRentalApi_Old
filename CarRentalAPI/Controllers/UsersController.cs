using AutoMapper;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRepositoryManager repository;

        public UsersController(IRepositoryManager repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersDomain = await repository.Users.GetAllAsync();

            var usersDTO = mapper.Map<List<Models.DTO.User>>(usersDomain);

            return Ok(usersDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDomain = await repository.Users.GetByIdAsync(id);

            var userDTO = mapper.Map<Models.DTO.User>(userDomain);

            return Ok(userDTO);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto updateRequest)
        {
            var user = await repository.Users.UpdateAsync(id, updateRequest);
            
            var userDto = mapper.Map<Models.DTO.User>(user);
            return Ok(userDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var userDomain = await repository.Users.DeleteAsync(id);

            if (userDomain is null)
                return NotFound();

            var userDTO = mapper.Map<Models.DTO.User>(userDomain);
            return Ok(userDTO);
        }

    }
}
