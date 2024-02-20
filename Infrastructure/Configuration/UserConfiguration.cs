using Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(user => user.SocialAccounts)
               .WithOne(sa => sa.User)
               .HasForeignKey(sa => sa.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.SecondaryEmails)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(u => u.PrimaryEmail)
            .IsUnique();

    }
}