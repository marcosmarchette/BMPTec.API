using BMPTec.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMPTec.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedNever();

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

           
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Endereco)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Email");

                email.HasIndex(e => e.Endereco)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuarios_Email");
            });


            builder.OwnsOne(u => u.CPF, cpf =>
            {
                cpf.Property(c => c.Numero)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("CPF");

                cpf.HasIndex(c => c.Numero)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuarios_CPF");
            });

            builder.Property(u => u.Telefone)
                .HasMaxLength(20);

            builder.Property(u => u.DataNascimento)
                .HasColumnType("date");

            builder.Property(u => u.DataCriacao)
                .IsRequired();

            builder.Property(u => u.DataAtualizacao);

            builder.Property(u => u.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

        
            builder.HasMany(u => u.Contas)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

    
            builder.HasIndex(u => u.Ativo)
                .HasDatabaseName("IX_Usuarios_Ativo");
        }
    }
}
