using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Auth.Queries.FindById;

public class FindUserByIdQueryHandler : IRequestHandler<FindUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;
    public FindUserByIdQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<User> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.Id);
        if (user is null)
            throw new UserNotFoundException();
        return user;
    }
}