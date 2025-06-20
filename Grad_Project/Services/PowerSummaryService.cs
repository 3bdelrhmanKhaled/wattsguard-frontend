using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Grad_Project.Services
{
    public class PowerSummaryService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PowerSummaryService> _logger;
        private readonly List<DeviceDatabase> _databases;
        private readonly string _fullDataDir;

        private static readonly Dictionary<string, double> SummerDefaultDevices = new Dictionary<string, double>
        {
            { "Television", 0.12 },
            { "Wi-Fi Router", 0.010 },
            { "Lighting (LED Bulbs × 8)", 0.086 },
            { "Chargers & Small Devices", 0.025 }
        };

        private static readonly Dictionary<string, double> WinterDefaultDevices = new Dictionary<string, double>
        {
            { "Television", 0.12 },
            { "Fans (2 units)", 0.17 },
            { "Wi-Fi Router", 0.010 },
            { "Lighting (LED Bulbs × 8)", 0.086 },
            { "Chargers & Small Devices", 0.025 }
        };

        private static readonly Dictionary<string, double> SummerCategoryPercentages = new Dictionary<string, double>
        {
            { "ElectricHeater", 0.10 },
            { "WaterHeater", 0.14 },
            { "Stove", 0.08 },
            { "Oven", 0.06 },
            { "Dishwasher", 0.04 },
            { "WashingMachine", 0.03 },
            { "ElectricalKattel", 0.05 },
            { "SteamIrons", 0.02 },
            { "Airfryer", 0.014 },
            { "Microwave", 0.02 },
            { "VaccumCleaners", 0.01 },
            { "Television", 0.024 },
            { "Wi-Fi Router", 0.016 },
            { "Lighting (LED Bulbs × 8)", 0.02 },
            { "Chargers & Small Devices", 0.045 }
        };

        private static readonly Dictionary<string, double> WinterCategoryPercentages = new Dictionary<string, double>
        {
            { "AirConditioners", 0.296 },
            { "WaterHeater", 0.04 },
            { "Stove", 0.08 },
            { "Oven", 0.06 },
            { "Dishwasher", 0.04 },
            { "WashingMachine", 0.03 },
            { "ElectricalKattel", 0.03 },
            { "SteamIrons", 0.02 },
            { "Airfryer", 0.014 },
            { "Microwave", 0.01 },
            { "VaccumCleaners", 0.01 },
            { "Television", 0.024 },
            { "Fans (2 units)", 0.03 },
            { "Wi-Fi Router", 0.016 },
            { "Lighting (LED Bulbs × 8)", 0.02 },
            { "Chargers & Small Devices", 0.045 }
        };

        public PowerSummaryService(IConfiguration configuration, ILogger<PowerSummaryService> logger, IWebHostEnvironment env)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var relativeDir = _configuration["DataSettings:DataDir"] ?? "wwwroot/data";
            _fullDataDir = Path.Combine(env.ContentRootPath, relativeDir);

            _logger.LogInformation($"Resolved data directory: {_fullDataDir}");
            _databases = InitializeDatabases();
        }

        private List<DeviceDatabase> InitializeDatabases()
        {
            if (!Directory.Exists(_fullDataDir))
            {
                _logger.LogError($"Data directory does not exist: {_fullDataDir}");
                throw new DirectoryNotFoundException($"Data directory does not exist: {_fullDataDir}");
            }

            string GetPath(string key, string fallback) =>
                Path.Combine(_fullDataDir, _configuration[$"DataSettings:{key}"] ?? fallback);

            var applianceFile = GetPath("ApplianceFile", "Electrical appliances.xlsx");
            var washingMachineFile = GetPath("WashingMachineFile", "washing machine.xlsx");
            var electricHeaterFile = GetPath("ElectricHeaterFile", "electrical heater.xlsx");
            var electricalKattelFile = GetPath("ElectricalKattelFile", "Electrical kattel.xlsx");
            var waterHeaterFile = GetPath("WaterHeaterFile", "water heater.xlsx");
            var airfryerFile = GetPath("AirfryerFile", "Airfryer.xlsx");
            var vacuumCleanersFile = GetPath("VacuumCleanersFile", "vaccum cleaners(2).xlsx");
            var dishwasherFile = GetPath("DishwasherFile", "dishwasher.xlsx");
            var heaterFile = GetPath("HeaterFile", "electrical heater.xlsx");
            var steamIronsFile = GetPath("SteamIronsFile", "electrical heater.xlsx");
            var microwaveOvenFile = GetPath("MicrowaveOvenFile", "Microwave & Oven.xlsx");
            var airConditionersFile = GetPath("AirConditionersFile", "Air Conditioners(2).xlsx");

            var databases = new List<DeviceDatabase>();

            void TryAdd<T>(string path, Func<string, ILogger, string, T> factory, string sheetName = null) where T : DeviceDatabase
            {
                if (File.Exists(path))
                {
                    databases.Add(factory(path, _logger, sheetName));
                    _logger.LogInformation($"Loaded: {Path.GetFileName(path)}");
                }
                else
                {
                    _logger.LogWarning($"Missing file: {path}");
                }
            }

            TryAdd(applianceFile, (f, l, s) => new RefrigeratorDatabase(f, l, s));
            TryAdd(washingMachineFile, (f, l, s) => new WashingMachineDatabase(f, l, s));
            TryAdd(electricHeaterFile, (f, l, s) => new ElectricHeaterDatabase(f, l, s), "Sheet2");
            TryAdd(electricalKattelFile, (f, l, s) => new ElectricalKattelDatabase(f, l, s));
            TryAdd(waterHeaterFile, (f, l, s) => new WaterHeaterDatabase(f, l, s));
            TryAdd(airfryerFile, (f, l, s) => new AirfryerDatabase(f, l, s));
            TryAdd(vacuumCleanersFile, (f, l, s) => new VacuumCleanersDatabase(f, l, s));
            TryAdd(dishwasherFile, (f, l, s) => new DishwasherDatabase(f, l, s));
            TryAdd(heaterFile, (f, l, s) => new heaterDatabase(f, l, s), "Sheet3");
            TryAdd(steamIronsFile, (f, l, s) => new SteamIronsDatabase(f, l, s), "Sheet1");
            TryAdd(airConditionersFile, (f, l, s) => new AirConditionersDatabase(f, l, s));

            if (File.Exists(microwaveOvenFile))
            {
                databases.Add(new OvenDatabase(microwaveOvenFile, _logger, "Sheet1")); // Updated to use Sheet1 as per image
                databases.Add(new MicrowaveDatabase(microwaveOvenFile, _logger, "Sheet2"));
                _logger.LogInformation("Loaded Microwave & Oven databases");
            }
            else
            {
                _logger.LogWarning($"Missing Microwave & Oven file: {microwaveOvenFile}");
            }

            _logger.LogInformation($"Total loaded databases: {databases.Count}");
            return databases;
        }

        public List<Dictionary<string, object>> GetPowerSummary(List<string> searchedModels, string season)
        {
            if (searchedModels == null || !searchedModels.Any())
            {
                _logger.LogWarning("Searched models list is null or empty.");
                return new List<Dictionary<string, object>>();
            }

            var powerData = new List<Dictionary<string, object>>();
            _logger.LogInformation($"Processing GetPowerSummary for season: {season}, models: {string.Join(", ", searchedModels)}");

            if (season.ToLower() == "summer")
            {
                var washingMachineDb = _databases.OfType<WashingMachineDatabase>().FirstOrDefault();
                if (washingMachineDb != null)
                {
                    var data = washingMachineDb.GetDeviceData();
                    foreach (var row in data)
                    {
                        var model = row.GetValueOrDefault("Model Name", "Unknown");
                        if (model == "EL1087567SLV" && searchedModels.Contains(model))
                        {
                            if (row.ContainsKey("Electric Power"))
                            {
                                if (double.TryParse(row["Electric Power"], out var power))
                                {
                                    powerData.Add(new Dictionary<string, object>
                                    {
                                        { "Category", "WashingMachine" },
                                        { "Electric Power", power },
                                        { "Model name", model }
                                    });
                                    _logger.LogInformation($"Added WashingMachine model {model} with power {power} kWh.");
                                }
                                else
                                {
                                    _logger.LogWarning($"Failed to parse Electric Power for model {model}: {row["Electric Power"]}");
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"Electric Power column missing for model {model}");
                            }
                        }
                    }
                }
            }

            foreach (var db in _databases)
            {
                var dbName = db.GetType().Name.Replace("Database", "");
                var data = db.GetDeviceData();
                foreach (var row in data)
                {
                    var model = row.GetValueOrDefault("Model Name", "Unknown");
                    if (season.ToLower() == "summer" && dbName == "WashingMachine" && model == "EL1087567SLV") continue;
                    if (searchedModels.Contains(model))
                    {
                        if (row.ContainsKey("Electric Power"))
                        {
                            if (double.TryParse(row["Electric Power"], out var power))
                            {
                                powerData.Add(new Dictionary<string, object>
                                {
                                    { "Category", dbName },
                                    { "Electric Power", power },
                                    { "Model name", model }
                                });
                                _logger.LogInformation($"Added {dbName} model {model} with power {power} kWh.");
                            }
                            else
                            {
                                _logger.LogWarning($"Failed to parse Electric Power for model {model}: {row["Electric Power"]}");
                            }
                        }
                        else if (dbName == "Oven") // Fallback for Oven
                        {
                            powerData.Add(new Dictionary<string, object>
                            {
                                { "Category", dbName },
                                { "Electric Power", 1.5 }, // Default power for ovens
                                { "Model name", model }
                            });
                            _logger.LogInformation($"Added {dbName} model {model} with default power 1.5 kWh (Electric Power missing).");
                        }
                        else
                        {
                            _logger.LogWarning($"Electric Power column missing for model {model} in {dbName}");
                        }
                    }
                }
            }

            var defaultDevices = season.ToLower() == "summer" ? SummerDefaultDevices : WinterDefaultDevices;
            foreach (var (device, power) in defaultDevices)
            {
                powerData.Add(new Dictionary<string, object>
                {
                    { "Category", device },
                    { "Electric Power", power },
                    { "Model name", "DEFAULT_DEVICES" }
                });
                _logger.LogInformation($"Added default device {device} with power {power} kWh.");
            }

            return powerData;
        }

        public List<Dictionary<string, object>> DistributePowerConsumption(double totalConsumption, List<Dictionary<string, object>> powerData, string season)
        {
            if (powerData == null || !powerData.Any())
            {
                _logger.LogWarning("Power data is null or empty.");
                return new List<Dictionary<string, object>>();
            }

            _logger.LogInformation($"Distributing power consumption: Total Consumption = {totalConsumption} kWh, Season = {season}");
            var categoryPercentages = season.ToLower() == "summer" ? SummerCategoryPercentages : WinterCategoryPercentages;
            var excludedCategories = new List<string> { "Wi-Fi Router", "Chargers & Small Devices", "Refrigerator", season.ToLower() == "winter" ? "WaterHeater" : null }
                .Where(c => c != null).ToList();

            var powerDf = powerData.Select(d => new
            {
                Category = (string)d["Category"],
                ModelName = (string)d["Model name"],
                ElectricPower = (double)d["Electric Power"]
            }).GroupBy(d => new { d.Category, d.ModelName })
              .Select(g => new Dictionary<string, object>
              {
                  { "Category", g.Key.Category },
                  { "Model name", g.Key.ModelName },
                  { "Electric Power", g.Sum(x => x.ElectricPower) },
                  { "Category Percentage", categoryPercentages.ContainsKey(g.Key.Category) ? categoryPercentages[g.Key.Category] / g.Count() : 0.0 }
              }).ToList();

            if (powerDf.Any(d => d["Category"].ToString() == "Refrigerator"))
            {
                foreach (var fridge in powerDf.Where(d => d["Category"].ToString() == "Refrigerator"))
                {
                    var fridgePower = (double)fridge["Electric Power"];
                    var fridgePercentage = totalConsumption != 0 ? fridgePower / totalConsumption : 0;
                    fridge["Category Percentage"] = fridgePercentage;
                    fridge["Monthly Consumption (kWh)"] = fridgePower;
                    _logger.LogInformation($"Adjusted Refrigerator: Power = {fridgePower}, Percentage = {fridgePercentage:P2}");
                }
            }

            var totalPercentage = powerDf.Sum(d => (double)d["Category Percentage"]);
            var remainingPercentage = Math.Max(0, 1 - totalPercentage);
            var adjustableCategories = powerDf.Where(d => !excludedCategories.Contains(d["Category"].ToString())).ToList();
            var numAdjustableCategories = adjustableCategories.Count;

            if (numAdjustableCategories > 0)
            {
                foreach (var d in adjustableCategories)
                {
                    d["Category Percentage"] = (double)d["Category Percentage"] + (remainingPercentage / numAdjustableCategories);
                }
                _logger.LogInformation($"Adjusted {numAdjustableCategories} categories with remaining percentage: {remainingPercentage:P2}");
            }

            foreach (var d in powerDf)
            {
                var percentage = (double)d["Category Percentage"];
                d["Category Percentage"] = Math.Round(percentage * 100, 2);
                d["Monthly Consumption (kWh)"] = Math.Round(percentage * totalConsumption, 2);
                var dailyHours = (double)d["Electric Power"] * 30 != 0 ? (double)d["Monthly Consumption (kWh)"] / ((double)d["Electric Power"] * 30) : 0;
                if (d["Category"].ToString() == "Refrigerator")
                {
                    dailyHours = 24;
                }
                dailyHours = Math.Min(Math.Max(dailyHours, 0), 24);
                d["Daily Hours"] = ElectricityBillCalculator.FormatHours(dailyHours);
                _logger.LogDebug($"Category: {d["Category"]}, Monthly Consumption: {d["Monthly Consumption (kWh)"]} kWh, Daily Hours: {d["Daily Hours"]}");
            }

            return powerDf.Select(d => new Dictionary<string, object>
            {
                { "Category", d["Category"] },
                { "Model name", d["Model name"] },
                { "Monthly Consumption (kWh)", d["Monthly Consumption (kWh)"] },
                { "Daily Hours", d["Daily Hours"] }
            }).ToList();
        }

        public List<DeviceDatabase> GetDatabases() => _databases;
    }

    public abstract class DeviceDatabase
    {
        protected readonly string FilePath;
        protected readonly ILogger Logger;
        protected readonly string SheetName;

        protected DeviceDatabase(string filePath, ILogger logger, string sheetName = null)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SheetName = sheetName;
        }

        public List<Dictionary<string, string>> GetDeviceData()
        {
            var data = new List<Dictionary<string, string>>();
            try
            {
                using var spreadsheetDocument = SpreadsheetDocument.Open(FilePath, false);
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheets = workbookPart.Workbook.Descendants<Sheet>().ToList();
                if (!sheets.Any())
                {
                    Logger.LogWarning($"No sheets found in file: {FilePath}");
                    return data;
                }

                WorksheetPart worksheetPart;
                if (SheetName == null)
                {
                    worksheetPart = workbookPart.WorksheetParts.First();
                }
                else
                {
                    var sheet = sheets.FirstOrDefault(s => s.Name == SheetName);
                    if (sheet == null)
                    {
                        Logger.LogWarning($"Sheet '{SheetName}' not found in file: {FilePath}");
                        return data;
                    }
                    worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                }

                var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                var rows = sheetData.Elements<Row>().ToList();

                if (rows.Count == 0)
                {
                    Logger.LogWarning($"No data in file: {FilePath}");
                    return data;
                }

                var headers = rows[0].Elements<Cell>().Select(c => GetCellValue(c, workbookPart)).ToList();
                foreach (var row in rows.Skip(1))
                {
                    var rowData = new Dictionary<string, string>();
                    var cells = row.Elements<Cell>().ToList();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        var value = cells.Count > i ? GetCellValue(cells[i], workbookPart) : "";
                        rowData[headers[i]] = value;
                    }
                    data.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error reading file: {FilePath}");
                throw;
            }
            return data;
        }

        public virtual string GetDeviceDetails(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                Logger.LogWarning("Model name is null or empty.");
                return null;
            }

            var data = GetDeviceData();
            var result = data.FirstOrDefault(row => row.ContainsKey("Model Name") && row["Model Name"].IndexOf(modelName, StringComparison.OrdinalIgnoreCase) >= 0);
            if (result == null)
            {
                Logger.LogDebug($"Model {modelName} not found in file: {FilePath}");
                return null;
            }

            var output = new List<string>();
            foreach (var (key, value) in result)
            {
                var unit = GetUnit(key);
                output.Add($"{key}: {value}{unit}");
            }
            return string.Join("\n", output);
        }

        protected virtual string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " L";
            if (columnLower.Contains("electric power")) return " kW";
            return "";
        }

        protected string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            if (cell == null || cell.CellValue == null) return "";

            var value = cell.CellValue.Text;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                var sharedStringTable = workbookPart.SharedStringTablePart?.SharedStringTable;
                if (sharedStringTable != null && int.TryParse(value, out var index))
                {
                    value = sharedStringTable.ElementAt(index).InnerText;
                }
            }
            return value;
        }
    }

    public class RefrigeratorDatabase : DeviceDatabase
    {
        public RefrigeratorDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }
    }

    public class WashingMachineDatabase : DeviceDatabase
    {
        public WashingMachineDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        public override string GetDeviceDetails(string modelName)
        {
            var data = GetDeviceData();
            var result = data.FirstOrDefault(row => row.ContainsKey("Model Name") && row["Model Name"] == modelName);
            if (result == null) return null;

            var output = new List<string>();
            foreach (var (key, value) in result)
            {
                var unit = key.ToLower() == "capacity" ? " L" : key.ToLower() == "electric power" ? " kWh" : "";
                output.Add($"{key}: {value}{unit}");
            }
            return string.Join("\n", output);
        }
    }

    public class ElectricHeaterDatabase : DeviceDatabase
    {
        public ElectricHeaterDatabase(string filePath, ILogger logger, string sheetName = "Sheet2") : base(filePath, logger, sheetName) { }

        public override string GetDeviceDetails(string modelName)
        {
            var data = GetDeviceData();
            var result = data.FirstOrDefault(row => row.ContainsKey("Model Name") && row["Model Name"] == modelName);
            if (result == null) return null;

            var output = new List<string>();
            foreach (var (key, value) in result)
            {
                var unit = key.ToLower() == "capacity" ? " H" : key.ToLower() == "electric power" ? " kWh" : "";
                output.Add($"{key}: {value}{unit}");
            }
            return string.Join("\n", output);
        }
    }

    public class ElectricalKattelDatabase : DeviceDatabase
    {
        public ElectricalKattelDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " L";
            if (columnLower.Contains("electric power")) return " W";
            return "";
        }
    }

    public class WaterHeaterDatabase : DeviceDatabase
    {
        public WaterHeaterDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }
    }

    public class AirfryerDatabase : DeviceDatabase
    {
        public AirfryerDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " L";
            if (columnLower.Contains("electric power")) return " W";
            return "";
        }
    }

    public class VacuumCleanersDatabase : DeviceDatabase
    {
        public VacuumCleanersDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " L";
            if (columnLower.Contains("electric power")) return " W";
            return "";
        }
    }

    public class DishwasherDatabase : DeviceDatabase
    {
        public DishwasherDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " places";
            if (columnLower.Contains("electric power")) return " kW";
            return "";
        }
    }

    public class heaterDatabase : DeviceDatabase
    {
        public heaterDatabase(string filePath, ILogger logger, string sheetName = "Sheet3") : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " places";
            if (columnLower.Contains("electric power")) return " kW";
            return "";
        }
    }

    public class SteamIronsDatabase : DeviceDatabase
    {
        public SteamIronsDatabase(string filePath, ILogger logger, string sheetName = "Sheet1") : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " N";
            if (columnLower.Contains("electric power")) return " W";
            return "";
        }
    }

    public class OvenDatabase : DeviceDatabase
    {
        public OvenDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " L";
            if (columnLower.Contains("electric power")) return " kW";
            return "";
        }
    }

    public class MicrowaveDatabase : DeviceDatabase
    {
        public MicrowaveDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        protected override string GetUnit(string column)
        {
            var columnLower = column.ToLower();
            if (columnLower.Contains("capacity")) return " L";
            if (columnLower.Contains("electric power")) return " kW";
            return "";
        }
    }

    public class AirConditionersDatabase : DeviceDatabase
    {
        public AirConditionersDatabase(string filePath, ILogger logger, string sheetName = null) : base(filePath, logger, sheetName) { }

        public override string GetDeviceDetails(string modelName)
        {
            var data = GetDeviceData();
            var result = data.FirstOrDefault(row => row.ContainsKey("Model Name") && row["Model Name"] == modelName);
            if (result == null) return null;

            var output = new List<string>();
            foreach (var (key, value) in result)
            {
                var unit = key.ToLower() == "capacity" ? " H" : key.ToLower() == "electric power" ? " kWh" : "";
                output.Add($"{key}: {value}{unit}");
            }
            return string.Join("\n", output);
        }
    }
}