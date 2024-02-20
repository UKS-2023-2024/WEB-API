using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.Delete;

public class DeleteRepositoryCommandHandler: ICommandHandler<DeleteRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IGitService _gitService;
    private readonly IUserRepository _userRepository;
    public DeleteRepositoryCommandHandler(
        IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IGitService gitService, IUserRepository userRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _gitService = gitService;
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteRepositoryCommand request, CancellationToken cancellationToken)
    {
        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.userId, request.repositoryId);
        if (member is null || member.Role != RepositoryMemberRole.OWNER)
            throw new UnautorizedAccessException();
        var repository = _repositoryRepository.Find(request.repositoryId);
        if (repository is null)
            throw new RepositoryNotFoundException(); 
        _repositoryRepository.Delete(repository);
        
        await _gitService.DeleteRepository(repository);
    }
}