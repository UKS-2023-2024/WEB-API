using Application.Shared;

namespace Application.Auth.Commands.Register;

public sealed record RegisterUserCommand(string primaryEmail,
    string password, string username, string fullName) : ICommand<string>;