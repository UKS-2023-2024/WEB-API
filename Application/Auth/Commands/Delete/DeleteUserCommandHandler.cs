using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories.Interfaces;

namespace Application.Auth.Commands.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IOrganizationRepository _organizationRepository;
    public DeleteUserCommandHandler(IUserRepository userRepository, IRepositoryRepository repositoryRepository, IOrganizationRepository organizationRepository)
    {
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
        _organizationRepository = organizationRepository;
    }
    public async Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.id);
        if (user is null)
            throw new UserNotFoundException();

        var userRepositories = await _repositoryRepository.FindAllByOwnerId(user.Id);
        var userOrganizations = await _organizationRepository.FindAllByOwnerId(user.Id);
        _userRepository.Delete(user);
        foreach(var repository in userRepositories) _repositoryRepository.Delete(repository);
        foreach(var organization in userOrganizations) _organizationRepository.Delete(organization);

        return user;
    }
}