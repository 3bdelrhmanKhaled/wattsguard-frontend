using AutoMapper;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRep scheduleRep;
        private readonly IMapper mapper;

        public ScheduleController(IScheduleRep scheduleRep,IMapper mapper)
        {
            this.scheduleRep = scheduleRep;
            this.mapper = mapper;
        }
        [HttpGet("GetAllSchedules")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var data = await scheduleRep.GetAllSchedulesAsync();
            if (data != null)
            {
                return Ok(data);

            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpGet("GetMySchedule")]
        public async Task<IActionResult> GetMySchedule(string userId)
        {
            var data = await scheduleRep.GetMyScheduleAsync(userId);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule(CreateScheduleDto scheduleDto)
        {
            var data = mapper.Map<Schedule>(scheduleDto);
            await scheduleRep.CreateScheduleAsync(data);
            return Ok("Created");
        }
        [HttpPut("UpdateMyScheduleById")]
        public async Task<IActionResult> UpdateMyScheduleById(int id,UpdateScheduleDto scheduleDto)
        {
            await scheduleRep.UpdateMyScheduleByIdAsync(id, scheduleDto);
            return Ok("Updated");

        }
        [HttpPut("UpdateMyScheduleByUserId")]
        public async Task<IActionResult> UpdateMyScheduleByUserId(string userId,UpdateScheduleDto scheduleDto)
        {
            await scheduleRep.UpdateMyScheduleByUserIdAsync(userId, scheduleDto);
            return Ok("Updated");

        }
        [HttpDelete("DeleteScheduleById")]
        public async Task <IActionResult> DeleteScheduleById(int id)
        {
            await scheduleRep.DeleteScheduleByIdAsync(id);
            return Ok("Deleted");
        }
        [HttpDelete("DeleteScheduleByUserId")]
        public async Task<IActionResult> DeleteScheduleByUserId(string userId)
        {
            await scheduleRep.DeleteScheduleByUserIdAsync(userId);
            return Ok("Deleted");
        }







    }
}
