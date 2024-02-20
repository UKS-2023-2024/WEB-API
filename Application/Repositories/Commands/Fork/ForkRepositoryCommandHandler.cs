using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.Fork;

public class ForkRepositoryCommandHandler : ICommandHandler<ForkRepositoryCommand, Guid>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGitService _gitService;
    private readonly IRepositoryForkRepository _repositoryForkRepository;

    public ForkRepositoryCommandHandler(IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IUserRepository userRepository,
        IGitService gitService,
        IRepositoryForkRepository repositoryForkRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _userRepository = userRepository;
        _gitService = gitService;
        _repositoryForkRepository = repositoryForkRepository;
    }

    public async Task<Guid> Handle(ForkRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        if (repository!.IsPrivate)
        {
            var repositoryMember =
                await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
            RepositoryMember.ThrowIfDoesntExist(repositoryMember);
        }

        var repositoryCheck = await _repositoryRepository.FindByNameAndOwnerId(repository.Name, request.UserId);
        if (repositoryCheck is not null)
            throw new YouAlreadyHaveRepositoryWithThisNameException();

        var creator = await _userRepository.FindUserById(request.UserId);
        User.ThrowIfDoesntExist(creator);

        var forkedRepository = Repository.Fork(repository, creator!);
        foreach (var branch in repository.Branches)
        {
            forkedRepository.AddBranch(Branch.Create(branch.Name,Guid.Empty, branch.IsDefault,creator!.Id));
        }
        
        var createdRepository = await _repositoryRepository.Create(forkedRepository);
        var repositoryFork = RepositoryFork.Create(repository, forkedRepository);
        await _repositoryForkRepository.Create(repositoryFork);
        
        await _gitService.ForkRepository(repository,creator!);
        
        return createdRepository.Id;
    }
}