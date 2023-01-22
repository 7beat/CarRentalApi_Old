using AutoMapper;
using CarRentalAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IMapper mapper;
        
        public VehiclesController(Repositories.IVehicleRepository vehicleRepository, IMapper mapper)
        {
            this.vehicleRepository = vehicleRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehiclesDomain = await vehicleRepository.GetAllAsync();

            var vehiclesDTO = mapper.Map<List<Models.DTO.Vehicle>>(vehiclesDomain);

            return Ok(vehiclesDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetVehicleById")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicleDomain = await vehicleRepository.GetByIdAsync(id);

            if (vehicleDomain is null)
                return NotFound();

            var vehicleDTO = mapper.Map<Models.DTO.Vehicle>(vehicleDomain);

            return Ok(vehicleDTO);
        }

        [HttpPost] //AddVehicle is required cuz i dont want to have id!
        public async Task<IActionResult> AddVehicle([FromBody] Models.DTO.AddVehicleRequest addVehicleRequest)
        {
            var vehicleDomain = new Models.Domain.Vehicle
            {
                Brand = addVehicleRequest.Brand,
                Model = addVehicleRequest.Model,
                ColorId = addVehicleRequest.Color,
                YearOfProduction = addVehicleRequest.YearOfProduction,
            };

            vehicleDomain = await vehicleRepository.AddAsync(vehicleDomain);

            var vehicleDTO = mapper.Map<Models.DTO.Vehicle>(vehicleDomain);

            //201
            return CreatedAtAction(nameof(GetVehicleById), new {id = vehicleDTO.Id}, vehicleDTO);
        }
    }
}
