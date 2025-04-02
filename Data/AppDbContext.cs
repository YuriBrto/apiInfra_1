using Microsoft.EntityFrameworkCore;
using apiInfra_1.Models;

namespace apiInfra_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Computer> Computers { get; set; }

        public DbSet<TpEquipamento> TipoEquipamento {get; set;}
    }
}
