using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Shared;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public Task<Branch?> FindById(Guid branchId)
        {
            return _context.Branches.Where(b => b.Id.Equals(branchId))
                .Include(b => b.Repository)
                .FirstOrDefaultAsync();
        }

        public async Task<Branch?> FindByRepositoryIdAndIsDefault(Guid repositoryId, bool isDefault)
        {
            return await _context.Branches
                    .Where(b => b.RepositoryId.Equals(repositoryId) && b.IsDefault == isDefault)
                    .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Branch>> FindAllByRepositoryIdAndIsDefault(Guid repositoryId, bool isDefault)
        {
            return await _context.Branches
                .Where(b => b.RepositoryId == repositoryId && b.IsDefault == isDefault && !b.Deleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> FindAllRepositoryBranches(Guid repositoryId)
        {
            return await _context.Branches
                .Where(b => b.RepositoryId == repositoryId && !b.Deleted)
                .ToListAsync();
        }

        public async Task<PagedResult<Branch>> FindAllByRepositoryIdAndDeletedAndIsDefault(Guid repositoryId, bool deleted, bool isDefault, int pageSize, int pageNumber)
        {
            var query = _context.Branches
                .Where(b => b.RepositoryId == repositoryId && b.Deleted == deleted && b.IsDefault == isDefault);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Branch>(data, totalItems);
        }

        public async Task<PagedResult<Branch>> FindAllByRepositoryIdAndOwnerIdAndDeletedAndIsDefault(Guid repositoryId, Guid ownerId, bool deleted, bool isDefault, int pageSize, int pageNumber)
        {
            var query = _context.Branches
                .Where(b => b.RepositoryId == repositoryId && b.OwnerId == ownerId && b.Deleted == deleted && b.IsDefault == isDefault);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Branch>(data, totalItems);
        }
    }
}
