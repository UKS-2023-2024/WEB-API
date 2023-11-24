using Domain.Branches;
using Domain.Branches.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Infrastructure.Branches
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        private readonly MainDbContext _context;
        public BranchRepository(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Branch?> FindByNameAndRepositoryId(string name, Guid repositoryId)
        {
            return await _context.Branches
                .Where(b => b.Name.Equals(name) && b.RepositoryId.Equals(repositoryId))
                .FirstOrDefaultAsync();
        }

        public async Task<Branch?> FindByRepositoryIdAndIsDefault(Guid repositoryId, bool isDefault)
        {
            return await _context.Branches
                    .Where(b => b.RepositoryId.Equals(repositoryId) && b.IsDefault == isDefault)
                    .FirstOrDefaultAsync();
        }
    }
}
