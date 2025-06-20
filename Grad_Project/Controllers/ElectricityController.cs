using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Grad_Project.Services;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment

namespace Grad_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricityController : ControllerBase
    {
        private readonly ILogger<ElectricityController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ModelLookupService _modelLookupService;
        private readonly PowerSummaryService _powerSummaryService;
        private readonly UserInputHandler _userInputHandler;
        private readonly IWebHostEnvironment _env;

        public ElectricityController(
            ILogger<ElectricityController> logger,
            IConfiguration configuration,
            ModelLookupService modelLookupService,
            PowerSummaryService powerSummaryService,
            UserInputHandler userInputHandler,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _configuration = configuration;
            _modelLookupService = modelLookupService ?? throw new ArgumentNullException(nameof(modelLookupService));
            _powerSummaryService = powerSummaryService ?? throw new ArgumentNullException(nameof(powerSummaryService));
            _userInputHandler = userInputHandler ?? throw new ArgumentNullException(nameof(userInputHandler));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        [HttpPost("calculate")]
        public IActionResult CalculateElectricity([FromQuery] string season, [FromBody] List<string> answers)
        {
            _logger.LogInformation($"Received request for season: {season}, payload: {JsonSerializer.Serialize(answers)}");

            if (season.ToLower() != "summer" && season.ToLower() != "winter")
            {
                _logger.LogWarning("Invalid season specified.");
                return BadRequest("Invalid season. Use 'summer' or 'winter'.");
            }

            if (answers == null || answers.Count == 0)
            {
                _logger.LogWarning("Missing input list.");
                return BadRequest(new { Success = false, Error = "Missing input list." });
            }

            if (!decimal.TryParse(answers[0], out var amount))
            {
                _logger.LogWarning("Invalid amount format.");
                return BadRequest(new
                {
                    Success = false,
                    Error = "Invalid amount format. Please enter a valid number."
                });
            }

            amount = Math.Round(amount, 2);

            if (amount >= 1720.6m && amount <= 2302.2m)
            {
                _logger.LogWarning("Amount out of valid range.");
                return BadRequest(new
                {
                    Success = false,
                    Error = "❌ Invalid input! The amount must be less than 1720.6 or greater than 2302.2."
                });
            }

            try
            {
                var totalConsumption = ElectricityBillCalculator.CalculateConsumption((double)amount);

                var results = _userInputHandler.ProcessInputs(answers, season);

                if (!results.Any() || !results.All(r => r.ContainsKey("Model Name")))
                {
                    _logger.LogWarning("No valid models were found or 'Model Name' is missing.");
                    return BadRequest(new
                    {
                        Success = false,
                        Error = "No valid models were found or 'Model Name' is missing."
                    });
                }

                var searchedModels = results.Select(r => r["Model Name"].ToString()).ToList();
                var powerSummary = _powerSummaryService.GetPowerSummary(searchedModels, season);
                var powerDf = _powerSummaryService.DistributePowerConsumption(totalConsumption, powerSummary, season);

                var output = new
                {
                    Success = true,
                    Summary = powerDf,
                    RawPowerData = powerSummary,
                    EstimatedBillRange = $"{amount} to {amount + 50}",
                    Message = season.ToLower() == "summer" ? "Avoid using devices from eliş6 to 9 PM" : "Avoid using devices from 6 to 10 PM"
                };

                _logger.LogInformation("Successfully processed electricity calculation.");
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing electricity calculation: {Message}\n{StackTrace}", ex.Message, ex.StackTrace);
                return StatusCode(500, new
                {
                    Success = false,
                    Error = "Unexpected error occurred.",
                    Exception = _env.IsDevelopment() ? ex.Message : null,
                    StackTrace = _env.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }

        [HttpGet("model-details")]
        public IActionResult GetModelDetails([FromQuery] string model)
        {
            _logger.LogInformation($"Received request for model details: {model}");

            // Validate the model parameter
            if (string.IsNullOrWhiteSpace(model))
            {
                _logger.LogWarning("Model parameter is missing or empty.");
                return BadRequest(new
                {
                    Success = false,
                    Error = "Model parameter is required and cannot be empty."
                });
            }

            try
            {
                var result = _modelLookupService.GetModelDetails(model.Trim());
                if (result == null)
                {
                    _logger.LogWarning($"No details found for model: {model}");
                    return NotFound(new
                    {
                        Success = false,
                        Error = $"No details found for model: {model}"
                    });
                }

                return Ok(new
                {
                    Success = true,
                    ModelDetails = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving model details: {Message}\n{StackTrace}", ex.Message, ex.StackTrace);
                return StatusCode(500, new
                {
                    Success = false,
                    Error = "Unexpected error occurred while retrieving model details.",
                    Exception = _env.IsDevelopment() ? ex.Message : null,
                    StackTrace = _env.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }
    }
}