using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto_Keep.Models.AutoKeep
{
    public class HistoricoVeiculos
    {
        [Key]
        public long Id_HistVeiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public DateTime? DataHora_Entrada { get; set; }
        public DateTime? DataHora_Saida { get; set; }
        public bool? Pago { get; set; }
        public bool? Excede_Hora { get => SetExcedeHora(); }
        public int Duracao { get => SetDuracaoHora(); }
        [NotMapped]
        public List<decimal?> Notas_Moedas { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Valor_Pago { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Valor_Troco { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Valor_Total { get; set; }
        [ForeignKey("TiposVeiculos")]
        public int? Id_TiposVeiculos { get; set; }
        public virtual TiposVeiculos TiposVeiculos { get; set; }

        public bool SetExcedeHora()
        {
            TimeSpan diferrenceDateTime = DataHora_Entrada.Value - DataHora_Saida.Value;
            return diferrenceDateTime.TotalMinutes > 60 ? true : false;
        }
        public int SetDuracaoHora()
        {
            TimeSpan diferrenceDateTime = DataHora_Entrada.Value - DataHora_Saida.Value;
            return diferrenceDateTime.Hours;
        }
    }
}
