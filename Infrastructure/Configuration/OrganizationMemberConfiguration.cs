﻿using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationMemberConfiguration: IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {

        builder.HasKey(member => new { member.OrganizationId, member.MemberId });
        builder
            .HasOne(mem => mem.Role)
            .WithMany(r => r.Members);
        builder.HasOne(mem => mem.Member)
            .WithMany(u => u.Members)
            .HasForeignKey(mem => mem.MemberId);
    }
}