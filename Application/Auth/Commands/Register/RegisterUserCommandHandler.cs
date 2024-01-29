using Application.Auth.Commands.Delete;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Shared.Interfaces;

namespace Application.Auth.Commands.Register;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IGitService _gitService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IHashingService hashingService, IGitService gitService)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _gitService = gitService;
    }
    
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindUserByEmail(request.PrimaryEmail);
        if (existingUser is not null)
            throw new UserWithThisEmailExistsException();
        var hashedPassword = _hashingService.Hash(request.Password);
        var registeredUser = User.Create(request.PrimaryEmail, request.FullName, request.Username,
            hashedPassword, UserRole.USER);
        var created = await _userRepository.Create(registeredUser);
        await _gitService.CreateUser(created, request.Password);
        return created.Id;
    }
}