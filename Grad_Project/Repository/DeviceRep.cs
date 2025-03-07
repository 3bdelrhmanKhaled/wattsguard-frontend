using AutoMapper;
using Grad_Project.Database;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Grad_Project.Services;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project.Repository
{
    public class DeviceRep : IDeviceRep
    {
        private readonly DataContext db;
        private readonly IPicService picService;
        private readonly IMapper mapper;

        public DeviceRep(DataContext db,IPicService picService,IMapper mapper)
        {
            this.db = db;
            this.picService = picService;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<Device>> GetAllDevicesAsunc()
        {
            var data = await db.devices.ToListAsync();
            return data;                
        }
        public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(string userId)
        {
            var data = await db.userDevices.Where(x => x.userId == userId).ToListAsync();
            var result = new List<Device>();
            foreach (var item in data)
            {
                var x = await db.devices.FindAsync(item.deviceId);
                result.Add(x);
            }
            return result;
        }
        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            var data = await db.devices.FindAsync(id);
            return data;
        }
        public async Task CreateDeviceAsync(Device device)
        {
            await db.devices.AddAsync(device);
            await db.SaveChangesAsync();
        }
        public async Task CreateDeviceToUserAsync(UserDevice userDevice)
        {
            await db.userDevices.AddAsync(userDevice);
            await db.SaveChangesAsync();

        }
        public async Task UpdateDeviceAsync(UpdateDeviceDto deviceDto)
        {
            var data = await db.devices.FindAsync(deviceDto.id);
            mapper.Map(deviceDto, data);
            if (deviceDto.file != null)
            {
                var picData = await picService.AddPicAsync(deviceDto.file);
                await picService.DeletePicAsync(data.picPublicId);
                data.picUrl = picData.Url.ToString();
                data.picPublicId = picData.PublicId;

                db.SaveChanges();

            }
            else
            {
                db.SaveChanges();


            }
        }
        public async Task EmptyDevicesByUserIdAsync(string userId)
        {
            var data = await db.userDevices.Where(x => x.userId == userId).ToListAsync();
            db.userDevices.RemoveRange(data);
            db.SaveChanges();
        }
        public async Task DeleteDeviceByIdAsync(int id,string publicId)
        {
            var data = await db.devices.FindAsync(id);
            db.devices.Remove(data);
            db.SaveChanges();
            await picService.DeletePicAsync(publicId);


        }

    }
}
