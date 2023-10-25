using Application.Auth.Commands.Delete;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;

namespace Application.Auth.Commands.Register;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    public RegisterUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;
    
    public Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User registeredUser = User.Create(request.primaryEmail, request.fullName, request.username,
            request.password, UserRole.USER, null, null, null, 
            null, null, null, false);
        _userRepository.Create(registeredUser);
        return Task.FromResult(registeredUser.Id.ToString());
    }
}