using Grad_Project.Database;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project.Repository
{
    public class AreaRep : IAreaRep
    {
        private readonly DataContext db;

        public AreaRep(DataContext db)
        {
            this.db = db;
        }
       
        public async Task<IEnumerable<Area>> GetAllAreaAsync()
        {
            var data = await db.areas.ToListAsync();
            return data;
        }

        public async Task<Area> GetAreaByIdAsync(int id)
        {
            var data = await db.areas.FindAsync(id);
            return data;
        }
        public async Task CreateAreaAsync(Area area)
        {
            await db.areas.AddAsync(area);
            await db.SaveChangesAsync();
            
        }

        public async Task UpdateAreaUsageAsync(int id,double usage)
        {
            var data = await db.areas.FindAsync(id);
            data.totalUsage = usage;
            await db.SaveChangesAsync();
        }
        public async Task DeleteAreaAsync(int id)
        {
            var data = await db.areas.FindAsync(id);
            db.areas.Remove(data);
            db.SaveChanges();

        }

    }
}
