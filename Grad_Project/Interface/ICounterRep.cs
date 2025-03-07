using Grad_Project.DTO;
using Grad_Project.Entity;

namespace Grad_Project.Interface
{
    public interface ICounterRep
    {
        Task<IEnumerable<Counter>> GetAllCountersAsync();
        Task<IEnumerable<CounterData>> GetCounterDataByCounterIdAync(string CounterId);
        Task CreateCounterDataAsync(CounterData counterData);
        Task UpdateCounterDataByIdAsync(int counterDataId, CreateCounterDataDto counterData);
        Task DeleteCounterDataByIdAsync(int id);
        Task EmptyCounterDataByCounterIdAsync(string counterId);
    }
}
