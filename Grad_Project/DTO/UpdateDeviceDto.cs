namespace Grad_Project.DTO
{
    public class UpdateDeviceDto
    {
        public int id { get; set; }
        public string ?name { get; set; }
        public double ?powerConsumption { get; set; }
        public IFormFile? file { get; set; }


    }
}
