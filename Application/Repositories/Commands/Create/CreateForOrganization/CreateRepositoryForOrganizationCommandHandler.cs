using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.Create.CreateForOrganization;

public class CreateRepositoryForOrganizationCommandHandler : ICommandHandler<CreateRepositoryForOrganizationCommand, Guid>
{
    private IUserRepository _userRepository;
    private IRepositoryRepository _repositoryRepository;
    private IOrganizationRepository _organizationRepository;
    private IGitService _gitService;

    public CreateRepositoryForOrganizationCommandHandler(IUserRepository userRepository, IRepositoryRepository repositoryRepository, IOrganizationRepository organizationRepository, IGitService gitService)
    {
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
        _organizationRepository = organizationRepository;
        _gitService = gitService;
    }

    public async Task<Guid> Handle(CreateRepositoryForOrganizationCommand request, CancellationToken cancellationToken)
    {    
        var organization = _organizationRepository.Find(request.OrganizationId);
        if (organization is null)
            throw new OrganizationNotFoundException();
        var existingRepository = await _repositoryRepository.FindByNameAndOrganizationId(request.Name, request.OrganizationId);
        if (existingRepository is not null)
            throw new RepositoryWithThisNameExistsException();

        var creator = await _userRepository.FindUserById(request.CreatorId);
        User.ThrowIfDoesntExist(creator);

        var repository = Repository.Create(request.Name, request.Description, request.IsPrivate, organization, creator!);
        repository.AddBranch(Branch.Create("main", Guid.Empty, true, creator.Id));

        repository = await _repositoryRepository.Create(repository);
        await _gitService.CreateOrganizationRepository(organization, repository);
        return repository.Id;
    }
}