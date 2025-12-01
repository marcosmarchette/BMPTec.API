using BMPTec.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMPTec.Infrastructure.Data.Configurations
{
    public class FeriadoConfiguration : IEntityTypeConfiguration<Feriado>
    {
        public void Configure(EntityTypeBuilder<Feriado> builder)
        {
            builder.ToTable("Feriados");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedNever();

            builder.Property(f => f.Data)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.TipoFeriado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(f => f.Recorrente)
                .IsRequired()
                .HasDefaultValue(false);

            // Índice único na data
            builder.HasIndex(f => f.Data)
                .IsUnique()
                .HasDatabaseName("IX_Feriados_Data");
        }
    }
}
