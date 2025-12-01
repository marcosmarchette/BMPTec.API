using BMPTec.Domain.Entities;
using BMPTec.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMPTec.Infrastructure.Data.Configurations
{
    public class TransferenciaConfiguration : IEntityTypeConfiguration<Transferencia>
    {
        public void Configure(EntityTypeBuilder<Transferencia> builder)
        {
            builder.ToTable("Transferencias");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.ContaOrigemId)
                .IsRequired();

            builder.Property(t => t.ContaDestinoId)
                .IsRequired();

            builder.Property(t => t.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Descricao)
                .HasMaxLength(200);

            builder.Property(t => t.DataTransferencia)
                .IsRequired();

            builder.Property(t => t.DataProcessamento);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(StatusTransferencia.Pendente);

            builder.Property(t => t.MotivoRejeicao)
                .HasMaxLength(500);

            builder.Property(t => t.TipoTransferencia)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue(TipoTransferencia.TED);

            // Relacionamento com Conta Origem
            builder.HasOne(t => t.ContaOrigem)
                .WithMany()
                .HasForeignKey(t => t.ContaOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Conta Destino
            builder.HasOne(t => t.ContaDestino)
                .WithMany()
                .HasForeignKey(t => t.ContaDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(t => t.ContaOrigemId)
                .HasDatabaseName("IX_Transferencias_ContaOrigemId");

            builder.HasIndex(t => t.ContaDestinoId)
                .HasDatabaseName("IX_Transferencias_ContaDestinoId");

            builder.HasIndex(t => t.DataTransferencia)
                .HasDatabaseName("IX_Transferencias_DataTransferencia");

            builder.HasIndex(t => t.Status)
                .HasDatabaseName("IX_Transferencias_Status");

        }
    }
}

