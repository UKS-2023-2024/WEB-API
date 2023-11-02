using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Exceptions;

namespace Application.Auth.Commands.Update;
/*
 * Handler predstavlja mjesto gdje se izvrsava funkcija koja prima komandu(poslate podatke).
 * Ovo se moze posmatrati kao najobicnija servisna funkcija ali sada skroz nezavisna u svojoj klasi.
 * Takodje, posto je komanda u pitanju, ne vraca se nista nazad
 * i trebalo bi da se rade CUD operacije(CREATE, UPDATE, DELETE) sa njom.
 */
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    public UpdateUserCommandHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.id);
        if (user is null)
            throw new UserNotFoundException();

        user.Update(request.fullName, request.bio, request.company, request.location, request.website, request.socialAccounts);
        _userRepository.Update(user);

        return user.Id.ToString();
    }
}