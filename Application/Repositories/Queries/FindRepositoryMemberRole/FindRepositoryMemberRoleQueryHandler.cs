using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindRepositoryMemberRole;

public class FindRepositoryMemberRoleQueryHandler: IRequestHandler<FindRepositoryMemberRoleQuery, RepositoryMemberRole?>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public FindRepositoryMemberRoleQueryHandler(IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task<RepositoryMemberRole?> Handle(FindRepositoryMemberRoleQuery request, CancellationToken cancellationToken)
    {
        var repositoryMember = await _repositoryMemberRepository
            .FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        return repositoryMember?.Role;
    }
}