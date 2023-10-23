
using Application.Shared;

namespace Application.Auth.Commands.Create;


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
public sealed record CreateUserCommand(string Email, string FullName): ICommand<string>;
