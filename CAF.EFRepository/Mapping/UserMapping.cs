using CAF.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAF.EFRepository.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");
            builder.Property(c => c.Id).HasColumnName("ID").UseIdentityColumn();
            builder.Property(c => c.FirstName).HasColumnName("FIRSTNAME").HasMaxLength(100).IsRequired();
            builder.Property(c => c.LastName).HasColumnName("LASTNAME").HasMaxLength(100);
            builder.Property(c => c.Email).HasColumnName("EMAIL").HasMaxLength(250).IsRequired();
            builder.Property(c => c.Password).HasColumnName("PASSWORD").HasMaxLength(250).IsRequired();
            builder.Property(c => c.RoleType).HasColumnName("ROLE_TYPE").IsRequired();
            builder.Property(c => c.DbState).HasColumnName("DBSTATE").IsRequired();
            builder.Property(c => c.ResetPasswordCode).HasColumnName("RESET_PASSWORD_CODE").HasMaxLength(200);
            builder.Property(c => c.ProfileImageUrl).HasColumnName("PROFILE_IMAGE_URL");
            builder.Property(c => c.SubRoles).HasColumnName("SUB_ROLES").HasMaxLength(50);
        }
    }
}
