using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Interface
{
    public interface IDeviceRep
    {
        Task<IEnumerable<Device>> GetAllDevicesAsunc();
        Task<IEnumerable<Device>> GetDevicesByUserIdAsync(string userId);
        Task<Device> GetDeviceByIdAsync(int id);
        Task CreateDeviceAsync(Device device);
        Task CreateDeviceToUserAsync(UserDevice userDevice);
        Task UpdateDeviceAsync(UpdateDeviceDto deviceDto);
        Task EmptyDevicesByUserIdAsync(string userId);
        Task DeleteDeviceByIdAsync(int id, string publicId);

    }
}
