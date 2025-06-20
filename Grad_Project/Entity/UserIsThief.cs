namespace Grad_Project.Entity
{
    public class UserIsThief
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign Key لـ AppUser
        public AppUser User { get; set; }
        public long ActualReading { get; set; }
        public DateTime ReadingDate { get; set; }
        public bool IsTheftReported { get; set; }
    }
}