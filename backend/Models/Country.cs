using backend.Models.enums;

namespace backend.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public int Population { get; set; }
        public Region Region { get; set; }
    }
}
