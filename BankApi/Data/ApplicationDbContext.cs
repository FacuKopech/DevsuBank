using Microsoft.EntityFrameworkCore;
using Model;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Reporte>();
            modelBuilder.Ignore<Resumen>();

            base.OnModelCreating(modelBuilder);

            //Persona Config
            modelBuilder.Entity<Persona>()
                .HasKey(p => p.Id);
            
            //Cliente Config
            modelBuilder.Entity<Cliente>()
                .HasBaseType<Persona>();

            modelBuilder.Entity<Cliente>()
              .Property(c => c.Estado)
              .HasConversion<string>();

            //Cuenta Config
            modelBuilder.Entity<Cuenta>()
                .HasKey(c => c.Id);
            
            modelBuilder.Entity<Cuenta>()
                .HasOne(c => c.Cliente)
                .WithMany(cu => cu.Cuentas) 
                .HasForeignKey(c => c.ClienteId);
            
            modelBuilder.Entity<Cuenta>()
            .Property(c => c.SaldoInicial)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Cuenta>()
              .Property(c => c.Estado)
              .HasConversion<string>();

            modelBuilder.Entity<Cuenta>()
                .Property(c => c.TipoCuenta)
                .HasConversion<string>();

            //Movimiento Config
            modelBuilder.Entity<Movimiento>()
                .HasKey(m => m.Id);
            
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Cuenta)
                .WithMany(m => m.Movimientos)
                .HasForeignKey(c => c.CuentaId);
            
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Saldo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Valor)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.TipoMovimiento)
                .HasConversion<string>();
        }
    }
}
