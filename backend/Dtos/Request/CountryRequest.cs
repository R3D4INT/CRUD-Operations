using backend.Models.enums;

namespace backend.Dtos.Request
{
    public class CountryRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Population { get; set; }
        public Region Region { get; set; }
    }
}
