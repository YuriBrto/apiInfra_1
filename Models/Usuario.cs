using System.ComponentModel.DataAnnotations;

namespace apiInfra_1.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        public string Role { get; set; } = "user"; // "admin" ou "user"
    }
}
