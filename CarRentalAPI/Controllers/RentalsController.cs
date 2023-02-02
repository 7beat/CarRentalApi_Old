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

            if (!ValidateAddRental(rentalDomain))
                return BadRequest(ModelState);

            rentalDomain = await rentalRepository.AddAsync(rentalDomain);

            if (rentalDomain is null)
                return BadRequest();

            var rentalDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            return CreatedAtAction(nameof(GetRentalById), new { id = rentalDto.Id }, rentalDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateRental(int id, [FromBody] Models.DTO.RentalUpdateRequest rentalAddRequest)
        {

            var rentalDomain = new Models.Domain.Rental
            {
                StartDate = rentalAddRequest.StartDate,
                EndDate = rentalAddRequest.EndDate
            };

            //var test = ValidateUpdateRental(rentalDomain);
            //true

            if (!ValidateUpdateRental(id, rentalDomain))
                return BadRequest(ModelState);

            rentalDomain = await rentalRepository.UpdateAsync(id, rentalDomain);

            if (rentalDomain is null)
                return BadRequest();

            var rentalDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            return CreatedAtAction(nameof(GetRentalById), new { id = rentalDto.Id }, rentalDto);
        }

        private bool ValidateAddRental(Models.Domain.Rental newRental)
        {
            if (newRental.UserId <= 0)
            {
                ModelState.AddModelError(nameof(newRental), $"{nameof(newRental.UserId)} needs to be specified.");
                return false;
            }

            if (newRental.VehicleId <= 0)
            {
                ModelState.AddModelError(nameof(newRental), $"{nameof(newRental.VehicleId)} needs to be specified.");
                return false;
            }

            if (newRental.StartDate.Day > DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))
            {
                ModelState.AddModelError(nameof(newRental), $"{nameof(newRental.StartDate)} needs to be proper day of a month.");
                return false;
            }

            foreach (var item in _appDbContext.Rentals.Where(x => x.Vehicle.Id == newRental.VehicleId)) // All rentals of given car
            {
                // Is a start or end of new rental between start and end of existing one?
                if (newRental.StartDate.IsInRange(item.StartDate, item.EndDate) || newRental.EndDate.IsInRange(item.StartDate, item.EndDate))
                {
                    // Dates are coliding
                    ModelState.AddModelError(nameof(newRental), $"Dates are coliding with {item.Id}");
                }

                // Is a start or end of existing rental between start and end of new one? New Rental cant happen while existing rental is happening!
                if (item.StartDate.IsInRange(newRental.StartDate, newRental.EndDate) || item.EndDate.IsInRange(newRental.StartDate, newRental.EndDate))
                {
                    // Dates are coliding, new rental is trying to be inserted over existing one!
                    ModelState.AddModelError(nameof(newRental), $"Dates are coliding with {item.Id}");
                }
            }

            // Result
            return ModelState.ErrorCount > 0 ? false : true;
        }

        private bool ValidateUpdateRental(int id, Models.Domain.Rental newRental)
        {
            var newRentalId = _appDbContext.Rentals.Find(id);

            // All rentals of given car excluding the modified one!
            foreach (var item in _appDbContext.Rentals.Where(x => x.Vehicle.Id == newRentalId.VehicleId && x.Id != newRentalId.Id))
            {
                // Is a start or end of new rental between start and end of existing one? New Rental cant start if the car is already rented!
                if (newRental.StartDate.IsInRange(item.StartDate, item.EndDate) || newRental.EndDate.IsInRange(item.StartDate, item.EndDate))
                {
                    // Dates are coliding
                    ModelState.AddModelError(nameof(newRental), $"Dates are coliding with {item.Id}");
                }

                // Is a start or end of existing rental beetwen start and end of new one? New Rental cant happen while existing rental is happening!
                if (item.StartDate.IsInRange(newRental.StartDate, newRental.EndDate) || item.EndDate.IsInRange(newRental.StartDate, newRental.EndDate))
                {
                    // Dates are coliding, new rental is trying to be inserted over existing one!
                    ModelState.AddModelError(nameof(newRental), $"Dates are coliding with {item.Id}");
                }
            }

            // Result
            return ModelState.ErrorCount > 0 ? false : true;
        }
    }
}
