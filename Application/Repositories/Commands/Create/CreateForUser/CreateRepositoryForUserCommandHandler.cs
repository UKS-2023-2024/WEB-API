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
using Domain.Tasks;

namespace Application.Repositories.Commands.Create.CreateForUser;

public class CreateRepositoryForUserCommandHandler : ICommandHandler<CreateRepositoryForUserCommand, Guid>
{
    private IUserRepository _userRepository;
    private IRepositoryRepository _repositoryRepository;
    private IGitService _gitService;

    public CreateRepositoryForUserCommandHandler(IUserRepository userRepository, IRepositoryRepository repositoryRepository, IGitService gitService)
    {
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
        _gitService = gitService;
    }

    public async Task<Guid> Handle(CreateRepositoryForUserCommand request, CancellationToken cancellationToken)
    {

        var existingRepository = await _repositoryRepository.FindByNameAndOwnerId(request.Name, request.CreatorId);
        if (existingRepository is not null)
            throw new RepositoryWithThisNameExistsException();

        var creator = await _userRepository.FindUserById(request.CreatorId);

        User.ThrowIfDoesntExist(creator);
        
        var repository = Repository.Create(request.Name, request.Description, request.IsPrivate, null,creator!);
        repository.AddBranch(Branch.Create("main", Guid.Empty, true, creator.Id));

        repository = await _repositoryRepository.Create(repository);
        repository.Labels.Add(new Label("enhancement", "New feature or request", "#a2eeef", repository.Id, true));
        repository.Labels.Add(new Label("bug", "Something isn't working", "#d73a4a", repository.Id, true));
        repository.Labels.Add(new Label("documentation", "Improvements or additions to documentation", "#0075ca", repository.Id, true));
        repository.Labels.Add(new Label("refactor", "Code is working but should be changed for reusability and readability", "#063846", repository.Id, true));
        repository.Labels.Add(new Label("auth", "Authorization and authentication", "#422F03", repository.Id, true));

        _repositoryRepository.Update(repository);
        
        // Git related updates
        var gitRepoData = await _gitService.CreatePersonalRepository(creator, repository);
        repository.SetCloneUrls(gitRepoData?.clone_url, gitRepoData?.ssh_url);
        _repositoryRepository.Update(repository);
        
        Console.WriteLine(gitRepoData?.clone_url);
        Console.WriteLine(gitRepoData?.ssh_url);
        Console.WriteLine(gitRepoData);
        
        return repository.Id;
    }
}