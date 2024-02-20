using Application.Auth.Commands.Update;
using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.Update;

public class UpdateRepositoryCommandHandler : ICommandHandler<UpdateRepositoryCommand, Repository>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IGitService _gitService;
    public UpdateRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository, IGitService gitService)
    {
       _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _gitService = gitService;
    }

    public async Task<Repository> Handle(UpdateRepositoryCommand request, CancellationToken cancellationToken)
    {
        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.userId, request.repositoryId);
        if (member is null || (member.Role != RepositoryMemberRole.OWNER && member.Role != RepositoryMemberRole.ADMIN))
            throw new UnautorizedAccessException();

        var repository = _repositoryRepository.Find(request.repositoryId);
        if (repository is null)
            throw new RepositoryNotFoundException();

        if (repository.Organization is not null)
        {
            var existingRepository = await _repositoryRepository.FindByNameAndOrganizationId(request.Name, repository.Organization.Id);
            if (existingRepository != null && existingRepository.Id != repository.Id)
                throw new RepositoryWithThisNameExistsException();
        }
        else
        {
            var repositoryOwner = await _repositoryMemberRepository.FindRepositoryOwner(request.repositoryId);
            var existingRepository = await _repositoryRepository.FindByNameAndOwnerId(request.Name, repositoryOwner.Member.Id);
            if (existingRepository != null && existingRepository.Id != repository.Id)
                throw new RepositoryWithThisNameExistsException();
        }

        var oldName = repository.Name;
        repository.Update(request.Name, request.Description, request.IsPrivate);
        _repositoryRepository.Update(repository);
        var defaultBranch = repository.Branches.Find(b => b.IsDefault == true);
        await _gitService.UpdateRepository(repository,defaultBranch!.OriginalName,oldName);
        
        return repository;
    }
}