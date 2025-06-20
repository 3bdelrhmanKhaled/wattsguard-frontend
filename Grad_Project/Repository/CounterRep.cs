using Grad_Project.Database;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Grad_Project.Repository
{
    public class CounterRep : ICounterRep
    {
        private readonly DataContext _context;
        private readonly ILogger<CounterRep> _logger;

        public CounterRep(DataContext context, ILogger<CounterRep> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Counter> GetCounterByCounterIdAsync(string counterId)
        {
            if (string.IsNullOrEmpty(counterId))
            {
                _logger.LogWarning("CounterId is null or empty.");
                return null;
            }

            var counter = await _context.counters
                .Include(c => c.User)
                .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(c => c.CounterId.ToLower() == counterId.ToLower());

            if (counter == null)
            {
                _logger.LogWarning($"No counter found with CounterId: {counterId}");
            }

            return counter;
        }

        public async Task CreateCounterDataAsync(CounterData counterData)
        {
            if (counterData == null)
            {
                _logger.LogError("CounterData is null.");
                throw new ArgumentNullException(nameof(counterData));
            }

            var counterExists = await _context.counters.AnyAsync(c => c.id == counterData.CounterId);
            if (!counterExists)
            {
                _logger.LogError($"No counter found with CounterId: {counterData.CounterId}");
                throw new InvalidOperationException($"لا يوجد عداد بالمعرف {counterData.CounterId} في جدول counters");
            }

            await _context.counterData.AddAsync(counterData);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"CounterData added successfully for CounterId: {counterData.CounterId}");
        }

        public async Task<bool> IsUserThiefByCounterIdAsync(string counterId)
        {
            var counter = await _context.counters
                .FirstOrDefaultAsync(c => c.CounterId.ToLower() == counterId.ToLower());
            if (counter == null)
            {
                _logger.LogWarning($"No counter found with CounterId: {counterId}");
                return false;
            }

            var latestCounterData = await _context.counterData
                .Where(d => d.CounterId == counter.id)
                .OrderByDescending(d => d.TimeStamp)
                .FirstOrDefaultAsync();

            if (latestCounterData == null)
            {
                _logger.LogWarning($"No CounterData found for CounterId: {counterId}");
                return false;
            }

            return latestCounterData.Flag == 1;
        }

        public async Task<IEnumerable<CounterData>> GetCounterDataByCounterIdAsync(string counterId)
        {
            var counter = await _context.counters
                .FirstOrDefaultAsync(c => c.CounterId.ToLower() == counterId.ToLower());
            if (counter == null)
            {
                _logger.LogWarning($"No counter found with CounterId: {counterId}");
                return new List<CounterData>();
            }

            return await _context.counterData
                .Include(d => d.Counter)
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.Address)
                .Where(d => d.CounterId == counter.id)
                .ToListAsync();
        }
    }
}