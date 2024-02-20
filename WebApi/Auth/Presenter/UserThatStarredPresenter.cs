using Domain.Auth;

namespace WEB_API.Auth.Presenter;

public class UserThatStarredPresenter
{
    public string Username { get; private set; }
    public string? Location { get; private set; }
    
    
    public UserThatStarredPresenter(User user)
    {
        Username = user.Username;
        Location = user.Location;
    }
    public static IEnumerable<UserThatStarredPresenter> MapUserToStarredPresenters(
        IEnumerable<User> usersThatStarred)
    {
        return usersThatStarred.Select(user => new UserThatStarredPresenter(user));
    }
}