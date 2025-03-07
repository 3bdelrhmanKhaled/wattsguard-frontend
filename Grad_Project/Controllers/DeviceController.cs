using AutoMapper;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Grad_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRep deviceRep;
        private readonly IPicService picService;
        private readonly IMapper mapper;

        public DeviceController(IDeviceRep deviceRep,IPicService picService,IMapper mapper)
        {
            this.deviceRep = deviceRep;
            this.picService = picService;
            this.mapper = mapper;
        }
        [HttpGet("GetAllDevices")]
        public async Task<IActionResult> GetAllDevices()
        {
            var data = await deviceRep.GetAllDevicesAsunc();
            if (data != null)
                return Ok(data);
            else
                return BadRequest("No Data");
        }
        [HttpGet("GetDevicesByUserId")]
        public async Task<IActionResult> GetDevicesByUserId(string userId)
        {
            var data = await deviceRep.GetDevicesByUserIdAsync(userId);
            if (data != null)
                return Ok(data);
            else
                return BadRequest("No Data");
        }
        [HttpGet("GetDeviceById")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            var data = await deviceRep.GetDeviceByIdAsync(id);
            if (data != null)
                return Ok(data);
            else
                return BadRequest("No Data");

        }
        [HttpPost("CreateDevice")]
        public async Task<IActionResult> CreateDevice(CreateDeviceDto deviceDto)
        {
            var picdata = await picService.AddPicAsync(deviceDto.file);
            var newDevice = mapper.Map<Device>(deviceDto);
            newDevice.picUrl = picdata.Url.ToString();
            newDevice.picPublicId = picdata.PublicId;
            await deviceRep.CreateDeviceAsync(newDevice);
            return Ok("New Device Added");

        }
        [HttpPost("CreateDeviceToUser")]
        public async Task<IActionResult> CreateDeviceToUser(UserDeviceDto userDevice)
        {
            var data = mapper.Map<UserDevice>(userDevice);
            await deviceRep.CreateDeviceToUserAsync(data);
            return Ok("Added");
            
        }
        [HttpPut("UpdateDevice")]
        public async Task<IActionResult> UpdateDevice(UpdateDeviceDto deviceDto)
        {
            await deviceRep.UpdateDeviceAsync(deviceDto);
            return Ok("Updated");

        }
        [HttpDelete("EmptyDevicesByUserId")]
        public async Task<IActionResult> EmptyDevicesByUserId(string userId)
        {
            await deviceRep.EmptyDevicesByUserIdAsync(userId);
            return Ok($"No Devices Related With UserId {userId}");
        }
        [HttpDelete("DeleteDevice")]
        public async Task<IActionResult> DeleteDevice(int id,string publicId)
        {
            await deviceRep.DeleteDeviceByIdAsync(id, publicId);

            return Ok("Deleted");

        }




    }
}
