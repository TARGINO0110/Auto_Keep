using System.ComponentModel.DataAnnotations;

namespace Auto_Keep.Models.AutoKeep
{
    public class TiposVeiculos
    {
        [Key]
        public int Id_TipoVeiculo { get; set; }
        public char? Sigla_Veiculo { get; set; }
        [MaxLength(100)]
        public string Tipo_Veiculo { get => SetDescVeiculo(); }
        public Precos Precos { get; set; }

        public string SetDescVeiculo()
        {
            return Sigla_Veiculo switch
            {
                'M' => "Moto",
                'C' => "Carro",
                'O' => "Ônibus",
                _ => "Desconhecido",
            };
        }
    }

    /// <summary>
    /// Moto - M / Carro - C / Ônibus - O
    /// </summary>
    public class PostTiposVeiculos 
    {
        public char? Sigla_Veiculo { get; set; }
    }

    /// <summary>
    /// Moto - M / Carro - C / Ônibus - O
    /// </summary>
    public class PutTiposVeiculos
    {
        public char? Sigla_Veiculo { get; set; }
    }
}
