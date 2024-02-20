using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Commands.Register;

public sealed record RegisterUserCommand(string PrimaryEmail,
    string Password, string Username, string FullName) : ICommand<Guid>;