using System.ComponentModel.DataAnnotations;

namespace apiInfra_1.Models
{
    public class Equipamento
    {
        public int Id { get; set; }

        [Required]
        public int MemoriaRam { get; set; }
        public int Armazenamento { get; set; }
        public string SistemaOperacional { get; set; }
        public string VersaoSO { get; set; }
        public string Modelo_dispositivo { get; set; }
        public string serial_numbeer { get; set; }
        public string Processador { get; set; }
        public string Placa_video{ get; set; }
        public string endereco_mac{ get; set; }

    }
}
