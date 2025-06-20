namespace Grad_Project.Entity
{
    public class OfficialReading
    {
        public int Id { get; set; }
        public string CounterId { get; set; }
        public double Reading { get; set; }
        public DateTime ReadingDate { get; set; }
        public Counter Counter { get; set; }
    }
}