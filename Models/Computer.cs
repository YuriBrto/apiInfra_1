using System.ComponentModel.DataAnnotations;

namespace apiInfra_1.Models
{
    public class Computer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }  // Notebook ou Desktop

        [Required]
        public string Setor { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        public int Ram { get; set; }

        [Required]
        public int Armazenamento { get; set; }

        public string TeamViewerId { get; set; }

        [Required]
        public string SistemaOperacional { get; set; }

        [Required]
        public string VersaoSO { get; set; }
    }
}
