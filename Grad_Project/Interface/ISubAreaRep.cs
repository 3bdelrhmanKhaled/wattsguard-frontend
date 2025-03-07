using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Interface
{
    public interface ISubAreaRep
    {
        Task<IEnumerable<SubArea>> GetAllSubAreasAsync();
        Task<IEnumerable<SubArea>> GetAllSubAreasByAreaIdAsync(int areaId);
        Task<SubArea> GetSubAreaByIdAsync(int id);
        Task CreateSubAreaAsync(SubArea subArea);
        Task UpdateSubAreaAsync(int id ,SubAreaDto subAreaDto);
        Task DeleteSubAreaAsync(int id);

    }
}
