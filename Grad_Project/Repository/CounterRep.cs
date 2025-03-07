using AutoMapper;
using Grad_Project.Database;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project.Repository
{
    public class CounterRep:ICounterRep
    {
        private readonly DataContext db;
        private readonly IMapper mapper;

        public CounterRep(DataContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<Counter>> GetAllCountersAsync()
        {
            var data = await db.counters.ToListAsync();
            return data;
        }

        public async Task<IEnumerable<CounterData>> GetCounterDataByCounterIdAync(string CounterId)
        {
            var data = await db.counterDatas.Where(x => x.counterId == CounterId).ToListAsync();
            return data;
        }
        public async Task CreateCounterDataAsync(CounterData counterData)
        {
            await db.counterDatas.AddAsync(counterData);
        }
        public async Task UpdateCounterDataByIdAsync(int counterDataId, CreateCounterDataDto counterData)
        {
            var data = await db.counterDatas.FindAsync(counterDataId);
            mapper.Map(counterDataId, data);
            await db.SaveChangesAsync();
        }
        public async Task DeleteCounterDataByIdAsync(int id)
        {
            var data = await db.counterDatas.FindAsync(id);
            db.counterDatas.Remove(data);
            db.SaveChanges();
        }

        public async Task EmptyCounterDataByCounterIdAsync(string counterId)
        {
            var data = await db.counterDatas.Where(x => x.counterId == counterId).ToListAsync();
            db.counterDatas.RemoveRange(data);
            db.SaveChanges();
        }
    }
}
