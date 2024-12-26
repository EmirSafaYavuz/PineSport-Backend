using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Concrete;

namespace DataAccess.Concrete.Configurations
{
    public class SchoolEntityConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            // Base sınıf (User) özelliklerini otomatik olarak dahil eder
            builder.Property(s => s.SchoolName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.SchoolAddress)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.SchoolPhone)
                .IsRequired()
                .HasMaxLength(20);

            // Şube ilişkisi
            builder.HasMany(s => s.Branches)
                .WithOne()
                .HasForeignKey(b => b.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tablo adı
            builder.ToTable("Schools");
        }
    }
}