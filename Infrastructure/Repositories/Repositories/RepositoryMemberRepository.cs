﻿using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryMemberRepository: BaseRepository<RepositoryMember>, IRepositoryMemberRepository
{

    private readonly MainDbContext _context;
    public RepositoryMemberRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }
    
    public Task<RepositoryMember?> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId)
    {
        return _context.RepositoryMembers
            .Where(r => r.Member.Id.Equals(userId) && r.RepositoryId.Equals(repositoryId))
            .FirstOrDefaultAsync();
    }

    public Task<RepositoryMember?> FindByRepositoryMemberIdAndRepositoryId(Guid repositoryMemberId, Guid repositoryId)
    {
        return _context.RepositoryMembers
            .Where(r => r.Id.Equals(repositoryMemberId) && r.RepositoryId.Equals(repositoryId))
            .FirstOrDefaultAsync();
    }

    public Task<RepositoryMember?> FindRepositoryOwner(Guid repositoryId)
    {
        return _context.RepositoryMembers
            .Include(r => r.Repository)
            .Include(r => r.Member)
            .Where(r => r.Repository.Id == repositoryId && r.Role == RepositoryMemberRole.OWNER)
            .FirstOrDefaultAsync();
    }

    public IEnumerable<RepositoryMember> FindRepositoryMembers(Guid repositoryId)
    {
        return _context.RepositoryMembers
            .Include(r => r.Member)
            .Where(r => r.RepositoryId == repositoryId && !r.Deleted)
            .AsEnumerable();
    }

    public int FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(Guid repositoryId,Guid repositoryMemberId)
    {
        return _context.RepositoryMembers
            .Include(r => r.Member)
            .Where(r => r.Role == RepositoryMemberRole.OWNER && r.RepositoryId == repositoryId && r.Id != repositoryMemberId && !r.Deleted)
            .AsEnumerable().Count();
    }
}