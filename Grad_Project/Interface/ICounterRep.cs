using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Interface
{
    public interface ICounterRep
    {
        Task<Counter> GetCounterByCounterIdAsync(string counterId);
        Task CreateCounterDataAsync(CounterData counterData);
        Task<bool> IsUserThiefByCounterIdAsync(string counterId);
        Task<IEnumerable<CounterData>> GetCounterDataByCounterIdAsync(string counterId);
    }
}