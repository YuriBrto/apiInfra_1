using System.ComponentModel.DataAnnotations;

namespace apiInfra_1.Models
{
    public class Usuario
    {
        public int ID_usuario { get; set; }
        public string username { get; set; }
        public string senha { get; set; } // Deveria ser hash na prática
    }
}
