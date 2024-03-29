﻿using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Enums;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Domain.Auth;
using Domain.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryRepository: BaseRepository<Repository>, IRepositoryRepository
{
    private readonly MainDbContext _context;
    public RepositoryRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public override Repository? Find(Guid id)
    {
        return _context.Repositories
            .Include(x => x.Organization)
            .ThenInclude(o => o.Members)
            .Include(x => x.Members)
            .ThenInclude(mem=>mem.Member)
            .Include(x=>x.StarredBy)
            .Include(x=>x.WatchedBy)
            .Include(x => x.Branches)
            .FirstOrDefault(x => x.Id == id);
    }
    public async Task<Repository?> FindByNameAndOwnerId(string name, Guid ownerId)
    {
        return await _context.Repositories
            .Include(r => r.Members)
            .Where(r => r.Name.ToLower().Equals(name.ToLower()) && r.Members.Any(m => (m.Member.Id == ownerId && m.Role == RepositoryMemberRole.OWNER)))
            .FirstOrDefaultAsync();
    }

    public async Task<Repository?> FindByNameAndOrganizationId(string name, Guid organizationId)
    {
        return await _context.Repositories
            .Include(r => r.Organization)
            .Where(r => r.Name.ToLower().Equals(name.ToLower()) && r.Organization.Id == organizationId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Repository>> FindAllByOwnerId(Guid id)
    {
        return _context.Repositories
            .Include(r => r.Members)
            .ThenInclude(m => m.Member)
            .Include(r => r.Organization)
            .Where(r => r.Organization == null && r.Members.Any(m => m.Member.Id == id && m.Role == RepositoryMemberRole.OWNER && !m.Deleted))
            .ToList();
    }

    public async Task<IEnumerable<Repository>> FindAllByOrganizationId(Guid id)
    {
        return _context.Repositories
            .Include(r => r.Members)
             .ThenInclude(m => m.Member)
            .Include(r => r.Organization)
            .Where(r => r.Organization.Id == id)
            .ToList();
    }

    public async Task<IEnumerable<Repository>> FindOrganizationRepositoriesThatContainsUser(Guid userId, Guid organizationId)
    {
        return _context.Repositories
            .Include(r => r.Members)
            .Where(repo => repo.Organization != null 
                           && repo.Organization.Id.Equals(organizationId) 
                           && repo.Members.Any(member => member.Member.Id.Equals(userId)))
            .ToList();
    }

    public Task<bool> DidUserStarRepository(Guid userid, Guid repositoryId)
    {
        return Task.FromResult(_context.Repositories
            .Include(r => r.StarredBy)
            .Count(r => r.Id.Equals(repositoryId) && r.StarredBy.Any(u => u.Id.Equals(userid))) == 1);
    }

    public async Task<IEnumerable<Repository>> FindAllUserBelongsTo(Guid id)
    {
        return _context.Repositories
            .Include(r => r.Members)
            .ThenInclude(m => m.Member)
            .Include(r => r.Organization)
            .Where(r => r.Members.Any(m => m.Member.Id == id && !m.Deleted))
            .ToList();
    }

    public Task<bool> IsUserWatchingRepository(Guid userid, Guid repositoryId)
    {
        return Task.FromResult(_context.Repositories
            .Include(r => r.WatchedBy)
            .Count(r => r.Id.Equals(repositoryId) && r.WatchedBy.Any(w => w.Id.Equals(userid) && w.WatchingPreferences == WatchingPreferences.AllActivity)) == 1);
    }

    public async Task<User?> FindRepositoryOwner(Guid repositoryId)
    {
        var member = await _context.RepositoryMembers
            .Where(member => member.RepositoryId.Equals(repositoryId) && member.Role == RepositoryMemberRole.OWNER)
            .Include(mem => mem.Member)
            .FirstOrDefaultAsync();
        return member?.Member;

    }

    public async Task<List<Label>> FindRepositoryLabels(Guid repositoryId, string searchValue)
    {
        return await _context.Labels
            .Where(l => l.RepositoryId.Equals(repositoryId) && l.IsDefaultLabel && !l.IsDeleted && (l.Title.ToLower().Contains(searchValue.ToLower()) ||
                    l.Description.ToLower().Contains(searchValue.ToLower())))
            .Include(l => l.Repository)
            .ToListAsync();
    }
    
    public override async Task<Repository> Create(Repository repository)
    {
        repository.Created();
        await _context.Set<Repository>().AddAsync(repository);
        await _context.SaveChangesAsync();
        return repository;
    }

    public override void Update(Repository repository)
    {
        repository.Updated();
        _context.Update(repository);
        _context.SaveChanges();
    }
}