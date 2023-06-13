using Auto_Keep.Models.AutoKeep;
using Microsoft.EntityFrameworkCore;

namespace Auto_Keep.Models.DbContextAutoKeep
{
    public class AutoKeepContext : DbContext
    {
        public AutoKeepContext(DbContextOptions<AutoKeepContext> options) : base(options)
        {
        }

        public virtual DbSet<EstoqueMonetario> EstoqueMonetarios { get; set; }
        public virtual DbSet<HistoricoVeiculos> HistoricoVeiculos { get; set; }
        public virtual DbSet<Precos> Precos { get; set; }
        public virtual DbSet<TiposVeiculos> TiposVeiculos { get; set; }
    }
}
