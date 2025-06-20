namespace Grad_Project.DTO
{
    public class CounterDataDto
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Reading { get; set; }
        public int Flag { get; set; }
        public string CounterId { get; set; }
        public bool IsTheftReported { get; set; }
        public AddressDto Address { get; set; }
    }
}