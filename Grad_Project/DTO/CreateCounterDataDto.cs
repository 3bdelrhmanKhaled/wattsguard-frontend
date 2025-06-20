namespace Grad_Project.DTO
{
    public class CreateCounterDataDto
    {
        public DateTime TimeStamp { get; set; }
        public double Reading { get; set; }
        public int Flag { get; set; }
        public string CounterId { get; set; }
    }
}