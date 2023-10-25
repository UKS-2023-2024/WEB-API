using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;

namespace Application.Auth.Commands.Create;
/*
 * Handler predstavlja mjesto gdje se izvrsava funkcija koja prima komandu(poslate podatke).
 * Ovo se moze posmatrati kao najobicnija servisna funkcija ali sada skroz nezavisna u svojoj klasi.
 * Takodje, posto je komanda u pitanju, ne vraca se nista nazad
 * i trebalo bi da se rade CUD operacije(CREATE, UPDATE, DELETE) sa njom.
 */
public class CreateUserCommandHandler: ICommandHandler<CreateUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    public CreateUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository; 
    
    public Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create("1231231231", "ana", "123", "123", UserRole.USER, "laal", "lala", "lala", "lala", new List<SocialAccount>() { new SocialAccount("123")}, new List<Email>() {new Email("123")}, false);
        _userRepository.Create(user);
        return Task.FromResult("123123123123");
    }
}