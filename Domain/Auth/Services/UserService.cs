using Library.Auth.Interfaces;
using Library.Enums;

namespace Library.Auth.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public List<User> FindAll()
    {
        return _userRepository.FindAll();
    }
}