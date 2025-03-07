namespace Grad_Project.Entity
{
    public class UserDevice
    {
        public string userId { get; set; }
        public AppUser user { get; set; }
        public int deviceId { get; set; }
        public Device device { get; set; }
    }
}
