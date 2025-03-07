namespace Grad_Project.Entity
{
    public class CounterData
    {
        public int id { get; set; }
        public DateTime timeStamp { get; set; }
        public double usage { get; set; }
        public string counterId { get; set; }
        public Counter counter { get; set; }
    }
}
