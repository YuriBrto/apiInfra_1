using System.ComponentModel.DataAnnotations;

namespace apiInfra_1.Models
{
    public class Maquina
    {
        public int ID_maquina { get; set; }
        public int FK_ID_setor { get; set; }
        public int FK_ID_usuario { get; set; }
        public int FK_ID_equipamento { get; set; }
        public string memoria_ram { get; set; }
        public string armazenamento { get; set; }
        public string sistema_opera { get; set; }
        public string versao_SO { get; set; }
        public string modelo_equipamento { get; set; }
        public string serial_number { get; set; }

        // Propriedades de navegação (opcional)
        public virtual Setor Setor { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual TpEquipamento TipoEquipamento { get; set; }
    }
}
