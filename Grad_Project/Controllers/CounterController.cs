using AutoMapper;
using Grad_Project.Database;
using Grad_Project.DTO;
using Grad_Project.Entity;
using Grad_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly ICounterRep _counterRep;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILogger<CounterController> _logger;

        public CounterController(ICounterRep counterRep, IMapper mapper, DataContext context, ILogger<CounterController> logger)
        {
            _counterRep = counterRep ?? throw new ArgumentNullException(nameof(counterRep));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("ProcessArduinoReading")]
        [AllowAnonymous]
        public async Task<IActionResult> ProcessArduinoReading([FromBody] CreateCounterDataDto readingDto)
        {
            if (readingDto == null || string.IsNullOrEmpty(readingDto.CounterId))
                return BadRequest("بيانات القراءة أو معرف العداد غير صالح.");

            var counter = await _counterRep.GetCounterByCounterIdAsync(readingDto.CounterId);
            if (counter == null)
                return NotFound($"لم يتم العثور على عداد بمعرف {readingDto.CounterId}.");

            _logger.LogInformation($"CounterId المستلم: {readingDto.CounterId}, Counter.Id المستخدم: {counter.id}");

            var counterData = new CounterData
            {
                CounterId = counter.id, 
                Reading = readingDto.Reading,
                TimeStamp = readingDto.TimeStamp,
                Flag = readingDto.Flag
            };

            await _counterRep.CreateCounterDataAsync(counterData);

            bool isTheft = counterData.Flag == 1;

            var response = _mapper.Map<CounterDataDto>(counterData);
            response.IsTheftReported = isTheft;

            return CreatedAtAction(nameof(GetCounterData), new { counterId = readingDto.CounterId }, response);
        }

        [HttpGet("GetCounterData/{counterId}")]
      //  [Authorize]
        public async Task<IActionResult> GetCounterData(string counterId)
        {
            if (string.IsNullOrEmpty(counterId))
                return BadRequest("معرف العداد مطلوب.");

            var counter = await _counterRep.GetCounterByCounterIdAsync(counterId);
            if (counter == null)
                return NotFound($"لم يتم العثور على عداد بمعرف {counterId}.");

            var records = await _context.counterData
                .Where(d => d.CounterId == counter.id)
                .ToListAsync();

            if (records == null || !records.Any())
                return NotFound($"لم يتم العثور على سجلات لمعرف العداد {counterId}.");

            var response = _mapper.Map<IEnumerable<CounterDataDto>>(records);
            foreach (var item in response)
            {
                item.IsTheftReported = item.Flag == 1;
            }

            return Ok(response);
        }

        [HttpGet("IsUserThief/{counterId}")]
        public async Task<IActionResult> IsUserThief(string counterId)
        {
            if (string.IsNullOrEmpty(counterId))
                return BadRequest("معرف العداد مطلوب.");

            var counter = await _counterRep.GetCounterByCounterIdAsync(counterId);
            if (counter == null)
                return NotFound($"لم يتم العثور على عداد بمعرف {counterId}.");

            var latestCounterData = await _context.counterData
                .Where(d => d.CounterId == counter.id)
                .OrderByDescending(d => d.TimeStamp)
                .FirstOrDefaultAsync();

            if (latestCounterData == null)
                return Ok(new { IsThief = false, Message = "لا توجد بيانات للعداد." });

            bool isThief = latestCounterData.Flag == 1;
            return Ok(new { IsThief = isThief, Message = isThief ? "المستخدم سارق." : "المستخدم ليس سارق." });
        }

        [HttpGet("GetUserAddress/{counterId}")]
   //     [Authorize]
        public async Task<IActionResult> GetUserAddress(string counterId)
        {
            if (string.IsNullOrEmpty(counterId))
                return BadRequest("معرف العداد مطلوب.");

            var counter = await _counterRep.GetCounterByCounterIdAsync(counterId);
            if (counter == null)
                return NotFound($"لم يتم العثور على عداد بمعرف {counterId}.");

            var address = _mapper.Map<AddressDto>(counter.User.Address);
            return Ok(address);
        }

        [HttpPost("AddOfficialReading")]
    //    [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOfficialReading([FromBody] OfficialReadingDto officialReadingDto)
        {
            if (officialReadingDto == null || string.IsNullOrEmpty(officialReadingDto.CounterId))
                return BadRequest("بيانات القراءة الرسمية غير صالحة.");

            var counter = await _context.counters.FirstOrDefaultAsync(c => c.CounterId == officialReadingDto.CounterId);
            if (counter == null)
                return NotFound($"لم يتم العثور على عداد بمعرف {officialReadingDto.CounterId}.");

            var officialReading = new OfficialReading
            {
                CounterId = counter.id, 
                Reading = officialReadingDto.Reading,
                ReadingDate = officialReadingDto.ReadingDate
            };

            await _context.officialReadings.AddAsync(officialReading);
            await _context.SaveChangesAsync();

            return Ok("تم إضافة القراءة الرسمية بنجاح.");
        }
    }
}