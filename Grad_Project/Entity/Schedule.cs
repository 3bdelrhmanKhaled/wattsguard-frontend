namespace Grad_Project.Entity
{
    public class Schedule
    {
        public int id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public bool isActive { get; set; } = false;
        public int deviceId { get; set; }
        public Device device { get; set; }
        public string userId { get; set; }
        public AppUser user { get; set; }
      
    }
}
