using Application.Shared;
using Domain.Auth;
using FluentResults;

namespace Application.Auth.Queries.FindById;

public sealed record FindUserByIdQuery(Guid Id) : IQuery<User>;