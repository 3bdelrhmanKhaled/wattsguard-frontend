using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Grad_Project.Services
{
    public class ModelLookupService
    {
        private readonly PowerSummaryService _powerSummaryService;
        private readonly ILogger<ModelLookupService> _logger;

        public ModelLookupService(PowerSummaryService powerSummaryService, ILogger<ModelLookupService> logger)
        {
            _powerSummaryService = powerSummaryService ?? throw new ArgumentNullException(nameof(powerSummaryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetModelDetails(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                _logger.LogWarning("Model name is null or empty.");
                return null;
            }

            try
            {
                var databases = _powerSummaryService.GetDatabases();
                if (databases == null)
                {
                    _logger.LogError("Databases not initialized in PowerSummaryService.");
                    throw new InvalidOperationException("Databases not initialized in PowerSummaryService.");
                }

                foreach (var db in databases)
                {
                    var details = db.GetDeviceDetails(model);
                    if (details != null)
                    {
                        _logger.LogInformation($"Found details for model: {model}");
                        return details;
                    }
                }

                _logger.LogWarning($"No details found for model: {model}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving details for model: {model}");
                throw; // Re-throw to be caught by the controller
            }
        }
    }
}