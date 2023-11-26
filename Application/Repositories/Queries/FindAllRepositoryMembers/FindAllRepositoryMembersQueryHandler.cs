using Application.Repositories.Queries.FindAllByOwnerId;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllRepositoryMembers;

public class FindAllRepositoryMembersQueryHandler : IRequestHandler<FindAllRepositoryMembersQuery, IEnumerable<RepositoryMember>>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    public FindAllRepositoryMembersQueryHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public Task<IEnumerable<RepositoryMember>> Handle(FindAllRepositoryMembersQuery request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        repository!.ThrowIfUserCantAccessRepositoryData(request.UserId);
        return Task.FromResult(_repositoryMemberRepository.FindRepositoryMembers(request.RepositoryId));
    }
}