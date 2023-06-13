using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto_Keep.Models.AutoKeep
{
    public class Precos
    {
        [Key]
        public int? Id_Preco { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Preco { get; set; }
        [ForeignKey("TiposVeiculos")]
        [Required(ErrorMessage = "Informe o tipo do veiculo para atribuição de preço!")]
        public int Id_TipoVeiculo { get; set; }
        public virtual TiposVeiculos TiposVeiculos { get; set; }
    }
}
