using AutoMapper;
using CarRentalAPI.Extensions;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        public RentalsController(IRepositoryManager repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all rentals", Description = "Retriving all rentals from Db as Dto")]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentalsDomain = await repository.Rentals.GetAllAsync();

            var rentalsDto = mapper.Map<IEnumerable<Models.DTO.Rental>>(rentalsDomain);

            return Ok(rentalsDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetRentalById")]
        [SwaggerOperation(Summary = "Retrieve single rental", Description = "Retrieve single rental by id as Dto")]
        public async Task<IActionResult> GetRentalById(int id)
        {
            var rentalDomain = await repository.Rentals.GetByIdAsync(id);

            if (rentalDomain is null)
                return NotFound();

            var rentalsDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            return Ok(rentalsDto);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add new rental",
            Description = "Add new rental, controller will validate if given object has set proper date and if it will colide with existing rental of given vehicle")]
        public async Task<IActionResult> AddRental([FromBody] Models.DTO.RentalAddRequest rentalAddRequest)
        {
            if (!await ValidateAddRental(rentalAddRequest))
                return BadRequest(ModelState);

            var rentalDomain = new Models.Domain.Rental
            {
                UserId = rentalAddRequest.UserId,
                VehicleId = rentalAddRequest.VehicleId,
                StartDate = rentalAddRequest.StartDate,
                EndDate = rentalAddRequest.EndDate
            };

            await repository.Rentals.AddAsync(rentalDomain);
            await repository.SaveAsync();

            // Get new object with relations
            rentalDomain = await repository.Rentals.GetByIdAsync(rentalDomain.Id);

            var rentalDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            //201
            return CreatedAtAction(nameof(GetRentalById), new { id = rentalDto.Id }, rentalDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [SwaggerOperation(Summary = "Modify rental",
            Description = "Modify existing rental, controller will validate if given object has proper date and if it's not coliding with existing rentals of given vehicle")]
        public async Task<IActionResult> UpdateRental(int id, [FromBody] Models.DTO.RentalUpdateRequest rentalAddRequest)
        {
            if (!await ValidateUpdateRental(id, rentalAddRequest))
                return BadRequest(ModelState);

            var rentalDomain = new Models.Domain.Rental
            {
                StartDate = rentalAddRequest.StartDate,
                EndDate = rentalAddRequest.EndDate
            };

            rentalDomain = await repository.Rentals.UpdateAsync(id, rentalDomain);

            if (rentalDomain is null)
                return BadRequest();

            await repository.SaveAsync();

            var rentalDto = mapper.Map<Models.DTO.Rental>(rentalDomain);

            return Ok(rentalDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [SwaggerOperation(Summary = "Delete rental", Description = "Delete rental by id")]
        public async Task<IActionResult> DeteleRental(int id)
        {
            var rentalDomain = await repository.Rentals.DeleteAsync(id);

            if (rentalDomain is null)
                return NotFound();

            await repository.SaveAsync();

            var rentalDto = mapper.Map<Models.DTO.Rental>(rentalDomain);
            return Ok(rentalDto);
        }

        private async Task<bool> ValidateAddRental(Models.DTO.RentalAddRequest newRental)
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

            if (newRental.StartDate > newRental.EndDate)
            {
                ModelState.AddModelError(nameof(newRental), $"{nameof(newRental.EndDate)} cant be in past.");
                return false;
            }

            var existingRentals = await repository.Rentals.GetAllAsync();

            foreach (var item in existingRentals.Where(x => x.Vehicle.Id == newRental.VehicleId)) // All rentals of given car
            {
                // Is a start or end of new rental between start and end of existing one?
                if (newRental.StartDate.IsInRange(item.StartDate, item.EndDate) || newRental.EndDate.IsInRange(item.StartDate, item.EndDate))
                {
                    // Dates are coliding, new rental is trying to be inserted under exising one!
                    ModelState.AddModelError(nameof(newRental), $"Rental is trying to be inserted under {item.Id}");
                }

                // Is a start or end of existing rental between start and end of new one? New Rental cant happen while existing rental is happening!
                if (item.StartDate.IsInRange(newRental.StartDate, newRental.EndDate) || item.EndDate.IsInRange(newRental.StartDate, newRental.EndDate))
                {
                    // Dates are coliding, new rental is trying to be inserted over existing one!
                    ModelState.AddModelError(nameof(newRental), $"Rental is trying to be inserted over {item.Id}");
                }
            }

            // Result
            return ModelState.ErrorCount > 0 ? false : true;
        }

        private async Task<bool> ValidateUpdateRental(int id, Models.DTO.RentalUpdateRequest newRental)
        {
            if (newRental.StartDate > newRental.EndDate)
            {
                ModelState.AddModelError(nameof(newRental), $"{nameof(newRental.EndDate)} cant be in past.");
                return false;
            }

            var newRentalDb = await repository.Rentals.GetByIdAsync(id);
            var existingRentals = await repository.Rentals.GetAllAsync();

            // All rentals of given car excluding the modified one!
            foreach (var item in existingRentals.Where(x => x.Vehicle.Id == newRentalDb.VehicleId && x.Id != newRentalDb.Id))
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
