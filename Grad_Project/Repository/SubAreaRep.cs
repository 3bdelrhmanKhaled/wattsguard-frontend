using AutoMapper;
using Grad_Project.Database;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.EntityFrameworkCore;

namespace Grad_Project.Repository
{
    public class SubAreaRep:ISubAreaRep
    {
        private readonly DataContext db;
        private readonly IMapper mapper;

        public SubAreaRep(DataContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<SubArea>> GetAllSubAreasAsync()
        {
            var data = await db.subAreas.ToListAsync();
            return data;
        
        }

        public async Task<IEnumerable<SubArea>> GetAllSubAreasByAreaIdAsync(int areaId)
        {
            var data = await db.subAreas.Where(x => x.areaId == areaId).ToListAsync();
            return data;
        }

        public async Task<SubArea> GetSubAreaByIdAsync(int id)
        {

            var data = await db.subAreas.FindAsync(id);
            return data;
        }
        public async Task CreateSubAreaAsync(SubArea subArea)
        {
            await db.subAreas.AddAsync(subArea);
            await db.SaveChangesAsync();

        }

        public async Task UpdateSubAreaAsync(int id,SubAreaDto subAreaDto)
        {
            var data = await db.subAreas.FindAsync(id);
            mapper.Map(subAreaDto, data);
           await  db.SaveChangesAsync();

        }
        public async Task DeleteSubAreaAsync(int id)
        {
            var data = await db.subAreas.FindAsync(id);
            db.subAreas.Remove(data);
            db.SaveChanges();

        }
    }
}
