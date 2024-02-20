using Domain.Auth;
using Domain.Auth.Interfaces;
using MediatR;

namespace Application.Auth.Queries.FindAllByStarredRepository;

public class FindAllByStarredRepositoryQueryHandler: IRequestHandler<FindAllByStarredRepositoryQuery, IEnumerable<User>>
{
    
    private readonly IUserRepository _userRepository;
    public FindAllByStarredRepositoryQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;
    
    public async Task<IEnumerable<User>> Handle(FindAllByStarredRepositoryQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.FindByStarredRepository(request.RepositoryId);
    }
}