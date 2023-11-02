using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Commands.Update;


/*
 * Komanda predstavlja objekat koji prenosi podatke do handlera(DTO).
 * U ovom slucaju potrebno je da naslijedi od ICommand interfejsa,
 * koji sam dodao, kako bi mogao MediatR da je serializuje.
 * Uglavnom nebitno, samo naslijediti svaku komandu sa ICommand.
 * Sto se tice paketiranja. Bukvalno se folder nazove kao operacija,
 * dok se klasama daju puna imena. Primjer:
 * Operacija => Create ili CreateUser
 * Komanda => CreateUserCommand
 * Handler => CreateUserCommandHandler ili CreateUserHandler
 * Mozemo se dogovoriti oko ovih konvencija. Takodje ista stvar vazi i za querije.
 * 
 */
public sealed record UpdateUserCommand(Guid id, string fullName, string bio, string company, string location, string website, List<SocialAccount> socialAccounts) : ICommand<User>;
