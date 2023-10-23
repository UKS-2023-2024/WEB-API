
using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Queries.FindAll;


/*
 * Kod querija, jedina razlika u odnosu na komande je sto se mora definisati povratni tip.
 * To je u ovom slucaju ovaj IEnumerable<User>
 * Sve ostalo je isto. Ima ulogu DTO objekta.
 * 
 */
public sealed record FindAllUsersQuery() : IQuery<IEnumerable<User>>;
