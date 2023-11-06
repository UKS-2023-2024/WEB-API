﻿using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RepositoryConfiguration: IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder
            .HasOne(r => r.Owner)         
            .WithMany(u => u.OwnedRepositories) 
            .HasForeignKey(r => r.OwnerId);

        builder
            .HasMany(o => o.Members)
            .WithOne(m => m.Repository)
            .HasForeignKey(o => o.RepositoryId);

        builder
            .HasMany(o => o.PendingMembers)
            .WithMany(u => u.PendingRepositories);
    }
}