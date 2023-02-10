using AutoMapper;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        public VehiclesController(IRepositoryManager repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all vehicles", Description = "Retrieve all vehicles in Dto form")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehiclesDomain = await repository.Vehicles.GetAllAsync();
            var vehiclesDto = mapper.Map<IEnumerable<Models.DTO.Vehicle>>(vehiclesDomain);

            return Ok(vehiclesDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetVehicleById")]
        [SwaggerOperation(Summary = "Retrieve single vehicle", Description = "Retrieve single vehicle by id in Dto form")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicleDomain = await repository.Vehicles.GetByIdAsync(id);

            if (vehicleDomain is null)
                return NotFound();

            var vehicleDto = mapper.Map<Models.DTO.Vehicle>(vehicleDomain);

            return Ok(vehicleDto);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Models.DTO.Vehicle), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Add new vehicle", Description = "Add new vehicle, display new object with all relations")]
        public async Task<IActionResult> AddVehicle([FromBody] Models.DTO.AddVehicleRequest addVehicleRequest)
        {
            var vehicleDomain = new Models.Domain.Vehicle
            {
                Brand = addVehicleRequest.Brand,
                Model = addVehicleRequest.Model,
                ColorId = addVehicleRequest.ColorId,
                YearOfProduction = addVehicleRequest.YearOfProduction,
            };

            await repository.Vehicles.AddAsync(vehicleDomain);
            await repository.SaveAsync();

            // Get new object with related Color
            var updatedVehicleDomain = await repository.Vehicles.GetByIdAsync(vehicleDomain.Id);

            var vehicleDto = mapper.Map<Models.DTO.Vehicle>(updatedVehicleDomain);

            // 201
            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicleDto.Id }, vehicleDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [SwaggerOperation(Summary = "Modify existing vehicle", Description = "Modify existing vehicle, Color could be changed.")]
        public async Task<IActionResult> UpdateVehicleAsync(int id,
            [FromBody] Models.DTO.UpdateVehicleRequest updateVehicleRequest)
        {
            if (!ValidateUpdateVehicle(updateVehicleRequest))
                return BadRequest(ModelState);

            var vehicleDomain = new Models.Domain.Vehicle
            {
                Model = updateVehicleRequest.Model,
                ColorId = updateVehicleRequest.Color,
            };

            vehicleDomain = await repository.Vehicles.UpdateAsync(id, vehicleDomain);

            if (vehicleDomain is null)
                return NotFound();

            await repository.SaveAsync();

            var vehicleDto = mapper.Map<Models.DTO.Vehicle>(vehicleDomain);

            return Ok(vehicleDto);
            //return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [SwaggerOperation(Summary = "Delete existing vehicle", Description = "Delete existing vehicle")]
        public async Task<IActionResult> DeleteVehicleAsync(int id)
        {
            var vehicleDomain = await repository.Vehicles.DeteleAsync(id);

            if (vehicleDomain is null)
                return NotFound();

            await repository.SaveAsync();

            var vehicleDto = mapper.Map<Models.DTO.Vehicle>(vehicleDomain);
            return Ok(vehicleDto);
            //return NoContent()
        }

        private bool ValidateUpdateVehicle(Models.DTO.UpdateVehicleRequest updateVehicleRequest)
        {
            if (updateVehicleRequest is null)
            {
                ModelState.AddModelError(nameof(updateVehicleRequest), $"{nameof(updateVehicleRequest)} cant be empty.");
                return false;
            }

            if (updateVehicleRequest.Color <= 0)
            {
                ModelState.AddModelError(nameof(updateVehicleRequest), $"{nameof(updateVehicleRequest.Color)} needs to be specified.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateVehicleRequest.Model))
            {
                ModelState.AddModelError(nameof(updateVehicleRequest), $"{nameof(updateVehicleRequest.Model)} is required.");
                return false;
            }

            //return ModelState.ErrorCount <= 0;
            //return ModelState.ErrorCount > 0 ? false : true;
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
    }
}
