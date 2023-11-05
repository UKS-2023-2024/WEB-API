using Application.Shared;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.Create;

public class CreateRepositoryCommandHandler: ICommandHandler<CreateRepositoryCommand, Guid>
{
    private IUserRepository _userRepository;
    private IRepositoryRepository _repositoryRepository;
    private IOrganizationRepository _organizationRepository;

    public CreateRepositoryCommandHandler(IUserRepository userRepository, IRepositoryRepository repositoryRepository, IOrganizationRepository organizationRepository)
    {
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<Guid> Handle(CreateRepositoryCommand request, CancellationToken cancellationToken)
    {
        Organization? organization = null;
        if (request.OrganizationId != Guid.Empty) {
            organization = _organizationRepository.Find(request.OrganizationId);
            if (organization is null)
                throw new OrganizationNotFoundException();
            var existingRepository = await _repositoryRepository.FindByNameAndOrganizationId(request.Name, request.OrganizationId);
            if (existingRepository is not null)
                throw new RepositoryWithThisNameExistsException();
        } else
        {
            var existingRepository = await _repositoryRepository.FindByNameAndOwnerId(request.Name, request.CreatorId);
            if (existingRepository is not null)
                throw new RepositoryWithThisNameExistsException();
        }

        var creator = await _userRepository.FindUserById(request.CreatorId);

        var repository = Repository.Create(request.Name, request.Description, request.IsPrivate, organization);

        var memberOwner = RepositoryMember.Create(creator, repository, RepositoryMemberRole.OWNER);
        repository.AddMember(memberOwner);
        
        repository = await _repositoryRepository.Create(repository);

        return repository.Id;
    }
}