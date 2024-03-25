using Microsoft.AspNetCore.Mvc;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectorController(IConnectorService connectorService) : ControllerBase
    {
        private readonly IConnectorService _connectorService = connectorService;

        [HttpGet]
        public async Task<ActionResult<Connector>> GetAllAsync()
        {
            return Ok(await _connectorService.GetAllAsync());
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> RemoveAsync(int id)
        {
            return Ok(await _connectorService.RemoveAsync(id));
        }

        [HttpGet("id")]
        public async Task<ActionResult<Connector>> GetByIdAsync(int id)
        {
            return Ok(await _connectorService.GetByIdAsync(id));
        }

        [HttpPost("chargeStationId")]
        public async Task<ActionResult<Connector>> AddAsync([Required] int chargeStationId, [Required]int newMaxCurrentInAmps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await _connectorService.AddAsync(chargeStationId, newMaxCurrentInAmps));
        }

        [HttpPut("id")]
        public async Task<ActionResult<Connector>> UpdateAsync([Required] int id, [Required] int newMaxCurrentInAmps)
        {
            return Ok(await _connectorService.UpdateAsync(id, newMaxCurrentInAmps));
        }
    }
}
