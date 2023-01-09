using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LoginProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
