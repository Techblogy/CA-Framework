using CAF.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAF.EFRepository.Mapping
{
    public class LogMapping : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("LOGS");
            builder.Property(c => c.Id).HasColumnName("ID").UseIdentityColumn();
            builder.Property(c => c.AppName).HasColumnName("APP_NAME").HasMaxLength(100);
            builder.Property(c => c.LogDate).HasColumnName("LOG_DATE").IsRequired();
            builder.Property(c => c.Username).HasColumnName("USERNAME").HasMaxLength(100);
            builder.Property(c => c.UserType).HasColumnName("USER_TYPE").HasMaxLength(100);
            builder.Property(c => c.UserId).HasColumnName("USER_ID");
            builder.Property(c => c.Type).HasColumnName("TYPE").HasMaxLength(250);
            builder.Property(c => c.Title).HasColumnName("TITLE").HasMaxLength(2000).IsRequired();
            builder.Property(c => c.Detail).HasColumnName("DETAIL");
        }
    }
}
