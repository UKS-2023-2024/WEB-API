using Domain.Auth;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WEB_API.Users.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string Fullname { get; set; }
    public string Username { get; set; }
    public string PrimaryEmail { get; set; }

    public UserDto(string id, string fullname, string username, string primaryEmail)
    {
        Id = id;
        Fullname = fullname;
        Username = username;
        PrimaryEmail = primaryEmail;
    }

    public static IEnumerable<UserDto> MapUsersToUserDtos(IEnumerable<User> users)
    {
        List<UserDto> userDtos = new List<UserDto>();
        foreach (User user in users)
        {
            userDtos.Add(new UserDto(user.Id.ToString(), user.FullName, user.Username, user.PrimaryEmail));
        }

        return userDtos;
    }
}