using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Grad_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChartsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ChartsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("devices-by-city")]
        public IActionResult GenerateDevicesChart()
        {
            try
            {
                // مسار سكربت Python
                string scriptPath = Path.Combine(_env.ContentRootPath, "PythonScripts", "devices_chart.py");

                // إعدادات تشغيل Python
                ProcessStartInfo psi = new()
                {
                    FileName = @"C:\Users\dell\AppData\Local\Programs\Python\Python313\python.exe", // المسار الصحيح
                    Arguments = $"\"{scriptPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = _env.ContentRootPath // المجلد الرئيسي للمشروع
                };

                // تشغيل Python
                using Process process = Process.Start(psi)!;
                process.WaitForExit();

                // لو في أخطاء، هتظهر هنا
                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    return StatusCode(500, $"Error running Python script: {error}");
                }

                // مسار الصورة الناتجة
                string relativePath = "charts/chart_devices_by_city.png";
                string filePath = Path.Combine(_env.WebRootPath, relativePath);

                // التأكد إن الصورة موجودة
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Chart image not found.");
                }

                // إرجاع رابط الصورة
                string imageUrl = $"{Request.Scheme}://{Request.Host}/{relativePath}";
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}