using Library.Auth;
using Library.Auth.Interfaces;
using Library.Enums;

namespace Infrastructure.Auth.Repositories;

public class UserRepository: IUserRepository
{
    public List<User> FindAll()
    {
        return new List<User>()
        {
            new User("1", "businessrdjan@gmail.com", "Srdjan Stjepanovic", UserRole.USER),
            new User("3", "businessrdjan@gmail.com", "Srdjan Stjepanovic", UserRole.USER),
            new User("2", "businessrdjan@gmail.com", "Srdjan Stjepanovic", UserRole.USER),
        };
    }

    public void Create(User user)
    {
        Console.WriteLine(user);
    }
}