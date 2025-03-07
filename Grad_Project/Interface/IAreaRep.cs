using Grad_Project.Entity;

namespace Grad_Project.Interface
{
    public interface IAreaRep
    {
        Task<IEnumerable<Area>> GetAllAreaAsync();
        Task<Area> GetAreaByIdAsync(int id);
        Task CreateAreaAsync(Area area);
        Task UpdateAreaUsageAsync(int id,double usage);
        Task DeleteAreaAsync(int id);
        
    }
}
