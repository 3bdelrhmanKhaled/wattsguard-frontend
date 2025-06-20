using System.Text.Json.Serialization;

namespace Grad_Project.DTO
{
    public class AddressDto
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("governorate")]
        public string Governorate { get; set; }
    }
}