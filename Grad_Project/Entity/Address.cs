namespace Grad_Project.Entity
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
    }
}