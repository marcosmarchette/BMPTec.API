using BMPTec.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMPTec.Infrastructure.Data.Configurations
{
    public class ExtratoConfiguration : IEntityTypeConfiguration<Extrato>
    {
        public void Configure(EntityTypeBuilder<Extrato> builder)
        {
            builder.ToTable("Extratos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.ContaId)
                .IsRequired();

            builder.Property(e => e.TipoOperacao)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(e => e.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.SaldoAnterior)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.SaldoAtual)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.DataOperacao)
                .IsRequired();

            builder.Property(e => e.Descricao)
                .HasMaxLength(200);

            builder.Property(e => e.TransferenciaId);

            builder.Property(e => e.NumeroDocumento)
                .HasMaxLength(50);

            // Relacionamento com Conta
            builder.HasOne(e => e.Conta)
                .WithMany()
                .HasForeignKey(e => e.ContaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Transferencia
            builder.HasOne(e => e.Transferencia)
                .WithMany()
                .HasForeignKey(e => e.TransferenciaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(e => e.ContaId)
                .HasDatabaseName("IX_Extratos_ContaId");

            builder.HasIndex(e => e.DataOperacao)
                .HasDatabaseName("IX_Extratos_DataOperacao");

            builder.HasIndex(e => new { e.ContaId, e.DataOperacao })
                .HasDatabaseName("IX_Extratos_ContaId_DataOperacao");

            builder.HasIndex(e => e.TransferenciaId)
                .HasDatabaseName("IX_Extratos_TransferenciaId");

            // Check constraint
            builder.HasCheckConstraint(
                "CK_Extratos_Valor",
                "[Valor] <> 0");
        }
    }
}
