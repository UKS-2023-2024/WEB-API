using Application.Auth.Queries.FindAll;
using Domain.Auth;
using Domain.Auth.Interfaces;
using MediatR;

namespace Application.Auth.Queries.Search;

public class SearchUsersQueryHandler: IRequestHandler<SearchUsersQuery, List<User>>
{
    private readonly IUserRepository _userRepository;

    public SearchUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<List<User>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        return _userRepository.SearchUsers(request.Value);
    }
}