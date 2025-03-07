using AutoMapper;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly ICounterRep counterRep;
        private readonly IMapper mapper;

        public CounterController(ICounterRep counterRep,IMapper mapper)
        {
            this.counterRep = counterRep;
            this.mapper = mapper;
        }
        [HttpGet("GetAllCounters")]
        public async Task<IActionResult> GetAllCounters()
        {
            var data = await counterRep.GetAllCountersAsync();
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpGet("GetCounterDataByCounterId")]
        public async Task<IActionResult> GetCounterDataByCounterId(string counterId)
        {
            var data = await counterRep.GetCounterDataByCounterIdAync(counterId);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpPost("CreateCounterData")]
        public async Task<IActionResult> CreateCounterData(CreateCounterDataDto counterDataDto)
        {
            var data = mapper.Map<CounterData>(counterDataDto);
            await counterRep.CreateCounterDataAsync(data);
            return Ok("Created");
        }
        [HttpPut("UpdateCounterDataById")]
        public async Task<IActionResult> UpdateCounterDataById(int counterDataId,CreateCounterDataDto counterDataDto)
        {
            await counterRep.UpdateCounterDataByIdAsync(counterDataId, counterDataDto);
            return Ok("Updated");
        }
        [HttpDelete("DeleteCounterDataById")]
        public async Task<IActionResult> DeleteCounterDataById(int counterDataId)
        {
            await counterRep.DeleteCounterDataByIdAsync(counterDataId);
            return Ok("Deleted");
        }
        [HttpDelete("EmptyCounterDataByCounterId")]
        public async Task<IActionResult> EmptyCounterDataByCounterId(string counterId)
        {
            await counterRep.EmptyCounterDataByCounterIdAsync(counterId);
            return Ok($"Counter Data of Counter with {counterId} is Empty.");

        }

    }
}
