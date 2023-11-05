using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;

namespace Application.Auth.Commands.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    public DeleteUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.id);
        if (user is null)
            throw new UserNotFoundException();

        user.Delete();
        _userRepository.Update(user);

        return user;
    }
}