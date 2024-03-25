using Microsoft.AspNetCore.Mvc;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartCharging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargeStationController(IChargeStationService chargeStationService) : ControllerBase
    {
        private readonly IChargeStationService _chargeStationService = chargeStationService;

        [HttpGet]
        public async Task<ActionResult<ChargeStation>> GetAllAsync()
        {
            return Ok(await _chargeStationService.GetAllAsync());
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> RemoveAsync([Required] int id)
        {
            return Ok(await _chargeStationService.RemoveAsync(id));
        }

        [HttpGet("id")]
        public async Task<ActionResult<ChargeStation>> GetByIdAsync([Required] int id)
        {
            return Ok(await _chargeStationService.GetByIdAsync(id));
        }

        [HttpPost("groupId")]
        public async Task<ActionResult<ChargeStationViewModel>> AddAsync(int groupId, ChargeStationPostModel chargeStationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await _chargeStationService.AddAsync(groupId, chargeStationViewModel));
        }

        [HttpPut("id")]
        public async Task<ActionResult<ChargeStationViewModel>> UpdateAsync([Required] int id, ChargeStationViewModel chargeStationViewModel)
        {
            return Ok(await _chargeStationService.UpdateAsync(id, chargeStationViewModel));
        }
    }
}
