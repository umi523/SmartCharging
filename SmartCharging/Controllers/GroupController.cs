using Microsoft.AspNetCore.Mvc;
using SmartChargingService.Interfaces;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;

namespace SmartCharging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController(IGroupService groupService) : ControllerBase
    {
        private readonly IGroupService _groupService = groupService;

        [HttpGet("id")]
        public async Task<ActionResult<Group>> GetByIdAsync(int id)
        {
            return Ok(await _groupService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<Group>> GetAllAsync()
        {
            return Ok(await _groupService.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Group>> AddAsync(GroupPostModel group)
        {
            return Ok(await _groupService.AddAsync(group));
        }

        [HttpPut("id")]
        public async Task<ActionResult<Group>> UpdateAsync(int id, Group group)
        {
            return Ok(await _groupService.UpdateAsync(id, group));
        }

        [HttpDelete("id")]
        public async Task<ActionResult<bool>> RemoveAsync(int id)
        {
            return Ok(await _groupService.RemoveAsync(id));
        }
    }
}
