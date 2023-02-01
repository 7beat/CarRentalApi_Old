using AutoMapper;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalRepository rentalRepository;
        private readonly IMapper mapper;
        public RentalsController(IRentalRepository repository, IMapper mapper) 
        {
            this.rentalRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentalsDomain = await rentalRepository.GetAllAsync();



            var rentalsDto = mapper.Map<IEnumerable<Models.DTO.Rental>>(rentalsDomain);

            return Ok(rentalsDto);
        }
    }
}
