﻿using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RepositoryConfiguration: IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder
            .HasMany(o => o.Members)
            .WithOne(m => m.Repository)
            .HasForeignKey(o => o.RepositoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(o => o.PendingInvites)
            .WithOne(u => u.Repository)
            .OnDelete(DeleteBehavior.Cascade);;
        
        builder
            .HasMany(o => o.StarredBy)
            .WithMany(u => u.Starred);
    }
}