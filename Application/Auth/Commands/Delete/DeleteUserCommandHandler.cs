using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Auth.Commands.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IGitService _gitService;
    public DeleteUserCommandHandler(IUserRepository userRepository, IRepositoryRepository repositoryRepository, IOrganizationRepository organizationRepository, IGitService gitService)
    {
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
        _organizationRepository = organizationRepository;
        _gitService = gitService;
    }
    public async Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.id);
        if (user is null)
            throw new UserNotFoundException();

        var userRepositories = await _repositoryRepository.FindAllByOwnerId(user.Id);
        var userOrganizations = await _organizationRepository.FindAllByOwnerId(user.Id);
        
        foreach (var repository in userRepositories)
        {
            await _gitService.DeleteRepository(repository);
            _repositoryRepository.Delete(repository);
        }

        foreach (var organization in userOrganizations)
        {
            await _gitService.DeleteOrganization(user,organization);
            _organizationRepository.Delete(organization);
        }
        
        await _gitService.DeleteUser(user);
        _userRepository.Delete(user);
        
        return user;
    }
}