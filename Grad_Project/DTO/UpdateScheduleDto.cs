namespace Grad_Project.DTO
{
    public class UpdateScheduleDto
    {
        public DateTime ?startTime { get; set; }
        public DateTime? endTime { get; set; }
        public bool ?isActive { get; set; }
    }
}
