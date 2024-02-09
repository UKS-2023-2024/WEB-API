using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.Fork;

public class ForkRepositoryCommandHandler : ICommandHandler<ForkRepositoryCommand, Guid>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGitService _gitService;

    public ForkRepositoryCommandHandler(IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IUserRepository userRepository,
        IGitService gitService)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _userRepository = userRepository;
        _gitService = gitService;
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

        var creator = await _userRepository.FindUserById(request.UserId);
        User.ThrowIfDoesntExist(creator);

        var forkedRepository = Repository.Fork(repository, creator!);
        foreach (var branch in repository.Branches)
        {
            forkedRepository.AddBranch(Branch.Create(branch.Name,Guid.Empty, branch.IsDefault,creator!.Id));
        }
        var createdRepository = await _repositoryRepository.Create(forkedRepository);
        await _gitService.ForkRepository(repository,creator!);
        return createdRepository.Id;
    }
}