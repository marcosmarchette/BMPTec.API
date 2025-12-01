using BMPTec.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMPTec.Infrastructure.Data.Configurations
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("Contas", t =>
            {
                // Check constraint para saldo
                t.HasCheckConstraint(
                    "CK_Contas_Saldo",
                    "[Saldo] >= -[LimiteChequeEspecial]");
            });

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.UsuarioId)
                .IsRequired();

            // Configuração do Value Object NumeroConta
            builder.OwnsOne(c => c.NumeroConta, numeroConta =>
            {
                numeroConta.Property(n => n.Numero)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("NumeroConta");

                numeroConta.Property(n => n.Agencia)
                    .IsRequired()
                    .HasColumnName("Agencia");

                // Índice único composto
                numeroConta.HasIndex(n => new { n.Numero, n.Agencia })
                    .IsUnique()
                    .HasDatabaseName("IX_Contas_NumeroConta_Agencia");
            });

            builder.Property(c => c.Saldo)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0.00);

            builder.Property(c => c.TipoConta)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(c => c.DataAbertura)
                .IsRequired();

            builder.Property(c => c.DataEncerramento);

            builder.Property(c => c.Ativa)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.LimiteChequeEspecial)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0.00);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            builder.Property(c => c.DataAtualizacao);

            // Relacionamento com Usuario
            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Contas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices adicionais
            builder.HasIndex(c => c.UsuarioId)
                .HasDatabaseName("IX_Contas_UsuarioId");

            builder.HasIndex(c => c.Ativa)
                .HasDatabaseName("IX_Contas_Ativa");

            builder.HasIndex(c => c.DataAbertura)
                .HasDatabaseName("IX_Contas_DataAbertura");
        }
    }
}
