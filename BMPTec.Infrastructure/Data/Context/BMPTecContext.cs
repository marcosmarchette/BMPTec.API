using BMPTec.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BMPTec.Infrastructure.Data.Context
{
    public class BmpTecContext : DbContext
    {
        public BmpTecContext(DbContextOptions<BmpTecContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Conta> Contas => Set<Conta>();
        public DbSet<Transferencia> Transferencias => Set<Transferencia>();
        public DbSet<Extrato> Extratos => Set<Extrato>();
        public DbSet<Feriado> Feriados => Set<Feriado>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações do assembly atual
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configura precisão decimal global
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            // Configura DateTime global para datetime2(7)
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                property.SetAnnotation("Relational:ColumnType", "datetime2(7)");
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Atualiza DataAtualizacao automaticamente
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is BMPTec.Domain.Common.AuditableEntity auditableEntity)
                {
                    auditableEntity.MarcarComoAtualizado();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
