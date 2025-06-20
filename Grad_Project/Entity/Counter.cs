namespace Grad_Project.Entity
{
    public class Counter
    {
        public string id { get; set; }
        public string userId { get; set; }
        public AppUser User { get; set; }
        public List<CounterData> CounterData { get; set; } = new List<CounterData>();
        public string CounterId { get; set; }
    }
}