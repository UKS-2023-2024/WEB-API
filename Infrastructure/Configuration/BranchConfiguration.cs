using Domain.Auth;
using Domain.Branches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {

            builder.HasOne(b => b.Repository)
                .WithMany(r => r.Branches)
                .HasForeignKey(b => b.RepositoryId);
            builder.HasIndex(b => new { b.Name, b.RepositoryId }).IsUnique();
        }
    }
}
