using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities.Concrete;

namespace DataAccess.Concrete.Configurations
{
    public class RoleClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            // Define composite key
            builder.HasKey(rc => new { rc.RoleId, rc.ClaimId });
        }
    }
}