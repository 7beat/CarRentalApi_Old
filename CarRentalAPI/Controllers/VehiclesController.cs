using AutoMapper;
using CarRentalAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
