namespace Grad_Project.DTO
{
    public class LoginResponseDto
    {
        public string id { get; set; }
        public string idNumber { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string counterId { get; set; }
        public DateTime lastLogin { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
