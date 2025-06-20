namespace Grad_Project.DTO
{
    public class UserIsThiefDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long ActualReading { get; set; }
        public DateTime ReadingDate { get; set; }
        public bool IsTheftReported { get; set; }
        public AddressDto Address { get; set; }
    }
}
