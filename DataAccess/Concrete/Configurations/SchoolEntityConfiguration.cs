using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities.Concrete;
using Core.Entities.Concrete;

namespace DataAccess.Concrete.Configurations
{
    public class SchoolEntityConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            /*
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

            // Branch ilişkisi güncellendi
            builder.HasMany(s => s.Branches)
                .WithOne(b => b.School)
                .HasForeignKey(b => b.SchoolId)
                .HasPrincipalKey(s => s.Id)  // User'dan gelen Id'yi kullan
                .OnDelete(DeleteBehavior.Cascade);

            // Tablo adı
            builder.ToTable("Schools");
            */

            builder.Property(s => s.SchoolName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.SchoolAddress)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(s => s.SchoolPhone)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.HasMany(s => s.Branches)
                .WithOne(b => b.School)
                .HasForeignKey(b => b.SchoolId)
                .HasPrincipalKey(s => s.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}