using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;

namespace Application.Auth.Commands.Delete;
/*
 * Handler predstavlja mjesto gdje se izvrsava funkcija koja prima komandu(poslate podatke).
 * Ovo se moze posmatrati kao najobicnija servisna funkcija ali sada skroz nezavisna u svojoj klasi.
 * Takodje, posto je komanda u pitanju, ne vraca se nista nazad
 * i trebalo bi da se rade CUD operacije(CREATE, UPDATE, DELETE) sa njom.
 */
public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    public DeleteUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = _userRepository.Find(request.id);
        user.Delete();
        _userRepository.Update(user);
        return Task.FromResult(user.Id.ToString());
    }
}