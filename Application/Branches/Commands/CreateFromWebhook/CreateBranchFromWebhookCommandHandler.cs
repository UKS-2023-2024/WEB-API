using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;

namespace Application.Branches.Commands.CreateFromWebhook;

public class CreateBranchFromWebhookCommandHandler: ICommandHandler<CreateBranchFromWebhookCommand>
{

    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public CreateBranchFromWebhookCommandHandler(IBranchRepository branchRepository,
        IRepositoryRepository repositoryRepository, IUserRepository userRepository,
        IOrganizationRepository organizationRepository)
    {
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
    }
    
    public async Task Handle(CreateBranchFromWebhookCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUsername(request.Username);
        var organization = await _organizationRepository.FindByName(request.Username);
        Repository repository;
        User owner;
        if (user is null && organization is null)
        {
            return;
        }
        if (user is not null)
        {
            owner = user;
            repository = await _repositoryRepository.FindByNameAndOwnerId(request.RepositoryName, user.Id);
            Repository.ThrowIfDoesntExist(repository);
        }
        else
        {
            repository = await _repositoryRepository.FindByNameAndOrganizationId(request.RepositoryName, organization.Id);
            Repository.ThrowIfDoesntExist(repository);
            owner = await _repositoryRepository.FindRepositoryOwner(repository.Id);
        }

        var branchName = request.RefName?[11..];
        var existingBranch = await _branchRepository.FindByNameAndRepositoryId(branchName, repository.Id);

        if (existingBranch is not null)
            return;

        var branch = Branch.Create(branchName, repository.Id, false, owner.Id);
        await _branchRepository.Create(branch);
    }
}