namespace Grad_Project.DTO
{
    public class CreateScheduleDto
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public bool isActive { get; set; }
        public int deviceId { get; set; }
        public string userId { get; set; }

    }
}
