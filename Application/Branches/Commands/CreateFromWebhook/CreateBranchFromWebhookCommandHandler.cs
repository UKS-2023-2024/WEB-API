using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;

namespace Application.Branches.Commands.CreateFromWebhook;

public class CreateBranchFromWebhookCommandHandler: ICommandHandler<CreateBranchFromWebhookCommand>
{

    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;

    public CreateBranchFromWebhookCommandHandler(IBranchRepository branchRepository,
        IRepositoryRepository repositoryRepository, IUserRepository userRepository)
    {
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
        _userRepository = userRepository;
    }
    
    public async Task Handle(CreateBranchFromWebhookCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUsername(request.Username);
        User.ThrowIfDoesntExist(user);

        var repository = await _repositoryRepository.FindByNameAndOwnerId(request.RepositoryName, user!.Id);
        Repository.ThrowIfDoesntExist(repository);

        var branchName = request.RefName?[11..];
        var existingBranch = await _branchRepository.FindByNameAndRepositoryId(branchName, repository.Id);

        if (existingBranch is not null)
            return;

        var branch = Branch.Create(branchName, repository.Id, false, user.Id);
        await _branchRepository.Create(branch);
    }
}