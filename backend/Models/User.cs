using System.ComponentModel.DataAnnotations;
using backend.Models;
using backend.Models.enums;

namespace backend
{
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
    }
}