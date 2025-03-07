using AutoMapper;
using Grad_Project.Database;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Grad_Project.Repository
{
    public class ScheduleRep:IScheduleRep
    {
        private readonly DataContext db;
        private readonly IMapper mapper;

        public ScheduleRep(DataContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        
       

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            var data = await db.schedules.ToListAsync();
            return data;
        }

        public async Task<Schedule> GetMyScheduleAsync(string userId)
        {
            var data = await db.schedules.Where(x => x.userId == userId).FirstOrDefaultAsync();
            return data;
        }
        public async Task CreateScheduleAsync(Schedule schedule)
        {
            await db.schedules.AddAsync(schedule);
            await db.SaveChangesAsync();
        }

        public async Task UpdateMyScheduleByIdAsync(int id, UpdateScheduleDto scheduleDto)
        {
            var data = await db.schedules.FindAsync(id);
            mapper.Map(scheduleDto, data);
            await db.SaveChangesAsync();
        }

        public async Task UpdateMyScheduleByUserIdAsync(string userId, UpdateScheduleDto scheduleDto)
        {
            var data = await db.schedules.Where(x => x.userId == userId).FirstOrDefaultAsync();
            mapper.Map(scheduleDto, data);
            await db.SaveChangesAsync();
        }
        public async Task DeleteScheduleByIdAsync(int id)
        {
            db.schedules.Remove(await db.schedules.FindAsync(id));
            await db.SaveChangesAsync();
        }

        public async Task DeleteScheduleByUserIdAsync(string userId)
        {
            var data = await db.schedules.Where(x => x.userId == userId).FirstOrDefaultAsync();
            db.schedules.Remove(data);
            await db.SaveChangesAsync();
        }
    }
}
