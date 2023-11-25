using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.Create.CreateForUser;

public class CreateRepositoryForUserCommandHandler : ICommandHandler<CreateRepositoryForUserCommand, Guid>
{
    private IUserRepository _userRepository;
    private IRepositoryRepository _repositoryRepository;

    public CreateRepositoryForUserCommandHandler(IUserRepository userRepository, IRepositoryRepository repositoryRepository)
    {
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Guid> Handle(CreateRepositoryForUserCommand request, CancellationToken cancellationToken)
    {

        var existingRepository = await _repositoryRepository.FindByNameAndOwnerId(request.Name, request.CreatorId);
        if (existingRepository is not null)
            throw new RepositoryWithThisNameExistsException();

        var creator = await _userRepository.FindUserById(request.CreatorId);
        User.ThrowIfDoesntExist(creator);
        
        var repository = Repository.Create(request.Name, request.Description, request.IsPrivate, null,creator!);

        repository = await _repositoryRepository.Create(repository);

        return repository.Id;
    }
}