using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;

namespace Application.Notifications.Commands.UpdateNotificationPreferences;

public class UpdateNotificationPreferencesCommandHandler : ICommandHandler<UpdateNotificationPreferencesCommand, User>
{
    private readonly IUserRepository _userRepository;
    public UpdateNotificationPreferencesCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(UpdateNotificationPreferencesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.userId);
        if (user is null)
            throw new UserNotFoundException();

        user.ChangeNotificationPreferences(request.preferences);
        _userRepository.Update(user);
       
        return user;
    }
}