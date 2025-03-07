using AutoMapper;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaRep areaRep;
        private readonly IMapper mapper;

        public AreaController(IAreaRep areaRep,IMapper mapper)
        {
            this.areaRep = areaRep;
            this.mapper = mapper;
        }
        [HttpGet("GetAllAreas")]
        public async Task<IActionResult> GetAllAreas()
        {
            var data = await areaRep.GetAllAreaAsync();
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest("No Data Founded");
            }
        }
        [HttpGet("GetAreaById")]
        public async Task<IActionResult> GetAreaById(int id)
        {
            var data = await areaRep.GetAreaByIdAsync(id);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest("No Data Founded");
            }

        }
        [HttpPost("CreateArea")]
        public async Task<IActionResult> CreateArea(CreateAreaDto areaDto)
        {
            var data = mapper.Map<Area>(areaDto);
            await areaRep.CreateAreaAsync(data);
            return Ok("Created");
        }
        [HttpPut("UpdateArea")]
        public async Task<IActionResult> UpdateAreaUsage(int id,double usage)
        {
            
            await areaRep.UpdateAreaUsageAsync(id,usage);
            return Ok("Updated");
        }
        [HttpDelete("DeleteAreaById")]
        public async Task<IActionResult> DeleteAreaById(int id)
        {
            await areaRep.DeleteAreaAsync(id);
            return Ok("Deleted");
        }
    }
}
