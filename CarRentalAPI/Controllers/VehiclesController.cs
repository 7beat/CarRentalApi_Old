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
        
        public VehiclesController(Repositories.IVehicleRepository vehicleRepository)
        {
            this.vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehiclesDomain = await vehicleRepository.GetAllAsync();

            //Map to DTO
            //var vehiclesDTO

            return Ok(vehiclesDomain);
        }
    }
}
