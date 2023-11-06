using Domain.Auth;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Auth.Queries.Login;

public class LoginQueryHandler: IRequestHandler<LoginQuery, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;

    public LoginQueryHandler(IUserRepository userRepository, IHashingService hashingService)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
    }

    public async Task<User> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmail(request.Email);
        if (user == null) throw new InvalidCredentialsException();
        
        if (!_hashingService.Verify(request.Password,user.Password))
            throw new InvalidCredentialsException();
        
        return user;
    }
}