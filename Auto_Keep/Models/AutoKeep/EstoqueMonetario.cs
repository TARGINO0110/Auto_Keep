using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto_Keep.Models.AutoKeep
{
    public class EstoqueMonetario
    {
        [Key]
        public int Id_Estoque { get; set; }
        public bool? Nota { get; set; }
        public bool? Moeda { get; set; }
        public string DescValor { get; set; }
        public int? Qtd { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Valor_Nota_Moeda { get; set; }
        public DateTime? Dt_Atualizado { get; set; }    
    }
}
