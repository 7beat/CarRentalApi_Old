using AutoMapper;
using CarRentalAPI.Data;
using CarRentalAPI.Extensions;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRentalRepository rentalRepository;
        private readonly IMapper mapper;
        public RentalsController(AppDbContext appDbContext, IRentalRepository repository, IMapper mapper) 
        {
            _appDbContext = appDbContext;
            rentalRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentalsDomain = await rentalRepository.GetAllAsync();

            var rentalsDto = mapper.Map<IEnumerable<Models.DTO.Rental>>(rentalsDomain);

            return Ok(rentalsDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetRentalById")]
        public async Task<IActionResult> GetRentalById(int id)
        {
            var rentalDomain = await rentalRepository.GetByIdAsync(id);

            if (rentalDomain is null)
                return NotFound();

            var rentalsDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            return Ok(rentalsDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddRental([FromBody] Models.DTO.RentalAddRequest rentalAddRequest)
        {

            var rentalDomain = new Models.Domain.Rental
            {
                UserId= rentalAddRequest.UserId,
                VehicleId= rentalAddRequest.VehicleId,
                StartDate= rentalAddRequest.StartDate,
                EndDate= rentalAddRequest.EndDate
            };

            if (!ValidateRentalDate(rentalDomain))
                return BadRequest(ModelState);

            rentalDomain = await rentalRepository.AddAsync(rentalDomain);

            if (rentalDomain is null)
                return BadRequest();

            var rentalDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            return CreatedAtAction(nameof(GetRentalById), new { id = rentalDto.Id }, rentalDto);
        }

        private bool ValidateRentalDate(Models.Domain.Rental newRental)
        {
            foreach (var item in _appDbContext.Rentals.Where(x => x.Vehicle.Id == newRental.VehicleId)) // All rentals of given car
            {
                // Is a start or end of new rental beetwen start and end of existing one?
                if (newRental.StartDate.IsInRange(item.StartDate, item.EndDate) || newRental.EndDate.IsInRange(item.StartDate, item.EndDate))
                {
                    // Dates are coliding
                    ModelState.AddModelError(nameof(newRental), $"Dates are coliding with {item.Id}");
                }
            }

            // Result
            return ModelState.ErrorCount > 0 ? false : true;
        }
    }
}
