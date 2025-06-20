using System.ComponentModel.DataAnnotations;

namespace Grad_Project.DTO
{
    public class RegisterDto
    {
        public string idNumber { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Phone]
        public string phone { get; set; }
        public string counterId { get; set; }
        public AddressDto address { get; set; } // إضافة العنوان
    }
}