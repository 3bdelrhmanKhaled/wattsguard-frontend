namespace Grad_Project.Entity
{
    public class CounterData
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Reading { get; set; }
        public int Flag { get; set; }
        public string CounterId { get; set; }
        public Counter Counter { get; set; }
    }
}