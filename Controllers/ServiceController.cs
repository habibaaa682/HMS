using HMS.Models;
using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class ServiceController : BaseBusinessController<Service, ServiceDto, IServiceServices>
    {
        private readonly IServiceServices _serviceServices;
        public ServiceController(IServiceServices serviceServices) {
            _serviceServices = serviceServices;
        }
        [HttpPost]
        public override async Task<IActionResult> Insert(ServiceDto service)
        {
            var createdService = await _serviceServices.Insert(service, UserId);
            if (createdService == null) return BadRequest("Service not created");
            return Ok(createdService);
        }
        [HttpPut]
        public override async Task<IActionResult> Edit(ServiceDto service)
        {
            var updatedService = await _serviceServices.Edit(service, UserId);
            if (updatedService == null) return BadRequest("Service not updated");
            return Ok(updatedService);
        }
        [HttpDelete]
        public override async Task<IActionResult> Remove(int serviceId)
        {
            var deletedService = await _serviceServices.Remove(serviceId, UserId);
            if (deletedService == false) return BadRequest("Service not deleted");
            return Ok(deletedService);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceServices.GetAllServices();
            if (services == null) return NotFound("No services found");
            return Ok(services);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var service = await _serviceServices.GetServiceById(serviceId);
            if (service == null) return NotFound("Service not found");
            return Ok(service);
        }
    }
}
