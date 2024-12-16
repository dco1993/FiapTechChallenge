using Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Configuration
{
    public class RegiaoConfiguration : IEntityTypeConfiguration<Regiao>
    {
        public void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.ToTable("REGIAO");
            builder.HasKey(r => r.Id);

            builder
                .Property(c => c.Id)
                .HasColumnType("INT")
                .HasColumnName("CD_REGIAO")
                .UseIdentityColumn();

            builder
                .Property(r => r.RegiaoDdd)
                .HasColumnType("INT")
                .HasColumnName("DDD_REGIAO")
                .IsRequired();

            builder
                .Property(r => r.Nome)
                .HasColumnType("VARCHAR(100)")
                .HasColumnName("NOME_REGIAO")
                .IsRequired();

        }
    }
}
