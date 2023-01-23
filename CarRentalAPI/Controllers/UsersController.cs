using AutoMapper;
using CarRentalAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository vehicleRepository;
        private readonly IMapper mapper;

        public UsersController(Repositories.IUserRepository vehicleRepository, IMapper mapper)
        {
            this.vehicleRepository = vehicleRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersDomain = await vehicleRepository.GetAllAsync();

            //Convert to DTO
            var usersDTO = mapper.Map<List<Models.DTO.User>>(usersDomain);

            return Ok(usersDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDomain = await vehicleRepository.GetByIdAsync(id);

            var userDTO = mapper.Map<Models.DTO.User>(userDomain);

            return Ok(userDTO);
        }
    }
}
