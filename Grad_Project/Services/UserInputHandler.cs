using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Grad_Project.Services
{
    public class UserInputHandler
    {
        private readonly PowerSummaryService _powerSummaryService;
        private readonly ILogger<UserInputHandler> _logger;
        private readonly List<Dictionary<string, string>> _results;

        public UserInputHandler(PowerSummaryService powerSummaryService, ILogger<UserInputHandler> logger)
        {
            _powerSummaryService = powerSummaryService ?? throw new ArgumentNullException(nameof(powerSummaryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _results = new List<Dictionary<string, string>>();
        }

        public List<Dictionary<string, string>> ProcessInputs(List<string> answers, string season)
        {
            int index = 1; // Skip amount
            var databases = _powerSummaryService.GetDatabases(); // Use the method

            if (databases == null)
            {
                _logger.LogError("Databases not initialized in PowerSummaryService.");
                throw new InvalidOperationException("Databases not initialized in PowerSummaryService.");
            }

            _logger.LogInformation($"Retrieved {databases.Count} databases.");
            foreach (var db in databases)
            {
                index = ReadDeviceInput(db, answers, index);
            }

            return _results;
        }

        private int ReadDeviceInput(DeviceDatabase db, List<string> answers, int index)
        {
            if (index >= answers.Count) return index;

            var hasDevice = answers[index].Trim().ToLower();
            index++;

            if (hasDevice == "yes")
            {
                if (index >= answers.Count)
                {
                    throw new ArgumentException($"Missing count for {db.GetType().Name}");
                }

                if (!int.TryParse(answers[index], out var count))
                {
                    throw new ArgumentException($"Invalid count format for {db.GetType().Name}");
                }

                index++;

                for (int i = 0; i < count; i++)
                {
                    if (index >= answers.Count)
                    {
                        throw new ArgumentException($"Missing model name for {db.GetType().Name}");
                    }

                    var model = answers[index];
                    var details = GetValidInput(db, model);
                    if (details != null)
                    {
                        var detailsDict = new Dictionary<string, string> { { "Model Name", model } };
                        foreach (var line in details.Split('\n'))
                        {
                            if (line.Contains(": "))
                            {
                                var parts = line.Split(": ", 2);
                                detailsDict[parts[0].Trim()] = parts[1].Trim();
                            }
                        }
                        _results.Add(detailsDict);
                    }
                    index++;
                }
            }

            return index;
        }

        private string GetValidInput(DeviceDatabase db, string model)
        {
            const int attempts = 2;
            for (int i = 0; i < attempts; i++)
            {
                var details = db.GetDeviceDetails(model);
                if (details != null) return details;
            }
            return null;
        }
    }
}