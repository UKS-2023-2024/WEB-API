using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Repositories.Exceptions;

namespace Application.Auth.Commands.Update;

public class UpdateRepositoryCommandHandler : ICommandHandler<UpdateRepositoryCommand, Repository>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    public UpdateRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
       _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task<Repository> Handle(UpdateRepositoryCommand request, CancellationToken cancellationToken)
    {
        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.userId, request.repositoryId);
        if (member is null || member.Role != RepositoryMemberRole.OWNER)
            throw new UnautorizedAccessException();

        var repository = _repositoryRepository.Find(request.repositoryId);
        if (repository is null)
            throw new RepositoryNotFoundException();

        if (repository.Organization is not null)
        {
            var existingRepository = await _repositoryRepository.FindByNameAndOrganization(request.Name, repository.Organization.Id);
            if (existingRepository != null && existingRepository.Id != repository.Id)
                throw new RepositoryWithThisNameExistsException();
        }
        else
        {
            var repositoryOwner = await _repositoryMemberRepository.FindRepositoryOwner(request.repositoryId);
            var existingRepository = await _repositoryRepository.FindByNameAndOwner(request.Name, repositoryOwner.Member.Id);
            if (existingRepository != null && existingRepository.Id != repository.Id)
                throw new RepositoryWithThisNameExistsException();
        }

        repository.Update(request.Name, request.Description, request.IsPrivate);
        _repositoryRepository.Update(repository);
        
        return repository;
    }
}