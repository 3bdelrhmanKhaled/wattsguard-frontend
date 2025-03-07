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
    public class SubAreaController : ControllerBase
    {
        private readonly ISubAreaRep subAreaRep;
        private readonly IMapper mapper;

        public SubAreaController(ISubAreaRep subAreaRep, IMapper mapper)
        {
            this.subAreaRep = subAreaRep;
            this.mapper = mapper;
        }
        [HttpGet("GetAllSubAreas")]
        public async Task<IActionResult> GetAllSubAreas()
        {
            var data = await subAreaRep.GetAllSubAreasAsync();
            if (data != null)
            {
                return Ok(data);

            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpGet("GetAllSubAreasByAreaId")]
        public async Task<IActionResult> GetAllSubAreasByAreaId(int areaId)
        {
            var data = await subAreaRep.GetAllSubAreasByAreaIdAsync(areaId);
            if (data != null)
            {
                return Ok(data);

            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpGet("GetSubAreaById")]
        public async Task<IActionResult> GetSubAreaById(int id)
        {
            var data = await subAreaRep.GetSubAreaByIdAsync(id);
            if (data != null)
            {
                return Ok(data);

            }
            else
            {
                return BadRequest("No Data");
            }
        }
        [HttpPost("CreateSubArea")]
        public async Task<IActionResult> CreateSubArea(SubAreaDto subAreaDto)
        {
            var data = mapper.Map<SubArea>(subAreaDto);
            await subAreaRep.CreateSubAreaAsync(data);
            return Ok("Created");
        }
        [HttpPut("UpdateSubArea")]
        public async Task<IActionResult> UpdateSubArea(int id, SubAreaDto subAreaDto)
        {
            await subAreaRep.UpdateSubAreaAsync(id, subAreaDto);
            return Ok("Updated");

        }
        [HttpDelete("DeleteSubAreaById")]
        public async Task<IActionResult> DeleteSubAreaById(int id)
        {
            await subAreaRep.DeleteSubAreaAsync(id);
            return Ok("Deleted");
        }
    }
}
