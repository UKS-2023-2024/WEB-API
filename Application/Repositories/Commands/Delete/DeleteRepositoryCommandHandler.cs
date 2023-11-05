using Application.Shared;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.Delete;

public class DeleteRepositoryCommandHandler: ICommandHandler<DeleteRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    public DeleteRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task Handle(DeleteRepositoryCommand request, CancellationToken cancellationToken)
    {
        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.userId, request.repositoryId);
        if (member is null || member.Role != RepositoryMemberRole.OWNER)
            throw new UnautorizedAccessException();
        Repository repository = _repositoryRepository.Find(request.repositoryId);
        _repositoryRepository.Delete(repository);
    }
}