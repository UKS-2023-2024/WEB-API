using Domain.Branches;
using Domain.Branches.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Branches
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(MainDbContext context) : base(context)
        {
        }
    }
}
