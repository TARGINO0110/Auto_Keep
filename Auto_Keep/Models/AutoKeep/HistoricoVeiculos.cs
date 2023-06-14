using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
            if (DataHora_Entrada.HasValue && DataHora_Saida.HasValue)
            {
                TimeSpan diferrenceDateTime = DataHora_Saida.Value - DataHora_Entrada.Value;
                return diferrenceDateTime.TotalMinutes > 60 ? true : false;
            }
            else
            {
                return false;
            }

        }
        public int SetDuracaoHora()
        {
            if (DataHora_Entrada.HasValue && DataHora_Saida.HasValue)
            {
                TimeSpan diferrenceDateTime = DataHora_Saida.Value - DataHora_Entrada.Value;
                return (int)diferrenceDateTime.TotalHours;
            }
            else { return 0; }
        }
    }

    public class PostHistoricoVeiculos 
    {
        public string PlacaVeiculo { get; set; }
        [JsonIgnore]
        public DateTime? DataHora_Entrada { get; set; }
        public int? Id_TiposVeiculos { get; set; }
    }


    public class PutHistoricoVeiculos
    {
        public string PlacaVeiculo { get; set; }
        public List<decimal?> Notas_Moedas { get; set; }
        public int? Id_TiposVeiculos { get; set; }
    }
}
