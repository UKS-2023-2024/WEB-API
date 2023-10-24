using Domain.Auth;
using Domain.Auth.Interfaces;
using MediatR;

namespace Application.Auth.Queries.FindAll;

/*
 * Razlika kod query handlera u odnosu na command handler je u tome sto vraca vrijednost.
 * Takodje ovaj custom interfejs prima 2 podatka umjesto jednog.
 * Prvi podatak je query koji se prosledjuje(DTO) a drugi podatak je povratni tip,
 * sto se mora poklapati sa onim definisanim u query klasi
 * 
 */
public class FindAllUsersQueryHandler: IRequestHandler<FindAllUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;
    public FindAllUsersQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public Task<IEnumerable<User>> Handle(FindAllUsersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userRepository.FindAll().AsEnumerable());
    }
}