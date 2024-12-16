using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Configuration
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("CONTATO");
            builder.HasKey(c => c.Id);

            builder
                .Property(c => c.Id)
                .HasColumnType("INT")
                .HasColumnName("CD_CONTATO")
                .UseIdentityColumn();

            builder
                .Property(c => c.NomeCompleto)
                .HasMaxLength(100)
                .HasColumnName("NOME_COMPLETO")
                .IsRequired();

            builder
                .Property(c => c.Email)
                .HasMaxLength(100)
                .HasColumnName("EMAIL")
                .HasAnnotation("RegularExpression", @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .IsRequired();

            builder
                .Property(c => c.TelefoneDdd)
                .HasMaxLength(2)
                .HasColumnName("TELEFONE_DDD")
                .IsRequired();

            builder
                .Property(c => c.TelefoneNum)
                .HasMaxLength(9)
                .HasColumnName("TELEFONE_NUMERO")
                .IsRequired();

            builder
                .HasOne(c => c.Regiao)
                .WithMany(r => r.Contatos)
                .HasForeignKey(c => c.TelefoneDdd)
                .HasPrincipalKey(r => r.RegiaoDdd);
        }
    }
}
