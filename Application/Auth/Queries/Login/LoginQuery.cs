using Application.Shared;
using Domain.Auth;
using FluentResults;
using MediatR;

namespace Application.Auth.Queries.Login;

public sealed record LoginQuery(string Email,
    string Password) : IQuery<User>;