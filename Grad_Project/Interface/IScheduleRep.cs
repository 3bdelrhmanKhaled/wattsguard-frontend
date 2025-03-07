using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Interface
{
    public interface IScheduleRep
    {
        Task<IEnumerable<Schedule>>GetAllSchedulesAsync();
        Task<Schedule> GetMyScheduleAsync(string userId);
        Task CreateScheduleAsync(Schedule schedule);
        Task UpdateMyScheduleByIdAsync(int id, UpdateScheduleDto scheduleDto);
        Task UpdateMyScheduleByUserIdAsync(string userId, UpdateScheduleDto scheduleDto);
        Task DeleteScheduleByIdAsync(int id);
        Task DeleteScheduleByUserIdAsync(string userId);


    }
}
