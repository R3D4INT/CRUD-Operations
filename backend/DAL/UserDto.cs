using System.ComponentModel.DataAnnotations;
using backend.Models.enums;

namespace backend.DAL
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
    }
}
