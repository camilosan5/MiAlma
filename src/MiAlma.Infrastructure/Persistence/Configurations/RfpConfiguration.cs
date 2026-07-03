using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiAlma.Domain.Entities;

namespace MiAlma.Infrastructure.Persistence.Configurations
{
    public class RfpConfiguration : IEntityTypeConfiguration<Rfp>
    {
        public void Configure(EntityTypeBuilder<Rfp> builder)
        {
            builder.ToTable("Rfps");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(r => r.Agency)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Description)
                .IsRequired();

            builder.Property(r => r.Deadline)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.HasMany(r => r.Proposals)
                .WithOne(p => p.Rfp)
                .HasForeignKey(p => p.RfpId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
