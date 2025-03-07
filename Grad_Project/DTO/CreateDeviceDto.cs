namespace Grad_Project.DTO
{
    public class CreateDeviceDto
    {
        public string name { get; set; }
        public double powerConsumption { get; set; }
        public IFormFile file { get; set; }

    }
}
