using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations;

public class BranchEntityConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        /*
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.BranchName).IsRequired();
        builder.Property(b => b.BranchAddress).IsRequired();
        builder.Property(b => b.BranchPhone).IsRequired();

        builder.HasOne(b => b.School)
            .WithMany(s => s.Branches)
            .HasForeignKey(b => b.SchoolId)
            .HasPrincipalKey(s => s.Id)  // School'un User'dan gelen Id'sini kullan
            .OnDelete(DeleteBehavior.Restrict);
            */

        builder.Property(b => b.BranchName).IsRequired();
        builder.Property(b => b.BranchAddress).IsRequired();
        builder.Property(b => b.BranchPhone).IsRequired();

        builder.HasOne(b => b.School)
            .WithMany(s => s.Branches)
            .HasForeignKey(b => b.SchoolId)
            .HasPrincipalKey(s => s.Id)  // School'un User'dan gelen Id'sini kullan
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Sessions)
            .WithOne(s => s.Branch)
            .HasForeignKey(s => s.BranchId)
            .HasPrincipalKey(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Students)
            .WithOne(s => s.Branch)
            .HasForeignKey(s => s.BranchId)
            .HasPrincipalKey(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}