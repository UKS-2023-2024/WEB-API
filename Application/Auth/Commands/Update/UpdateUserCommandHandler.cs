using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;

namespace Application.Auth.Commands.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly ISocialAccountRepository _socialAccountRepository;
    public UpdateUserCommandHandler(IUserRepository userRepository, ISocialAccountRepository socialAccountRepository)
    {
        _userRepository = userRepository;
        _socialAccountRepository = socialAccountRepository;
    }

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.id);
        if (user is null)
            throw new UserNotFoundException();

        var allSocialAccounts = user.SocialAccounts ?? new();
        user.Update(request.fullName, request.bio, request.company, request.location, request.website, request.socialAccounts);
        _userRepository.Update(user);
        foreach (SocialAccount acc in allSocialAccounts)
        {
            if (!request.socialAccounts.Any(x => x.Id == acc.Id))
                _socialAccountRepository.Delete(acc);

        }

        return user;
    }
}