using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Queries.Search;

public sealed record SearchUsersQuery(string Value) : IQuery<List<User>>;