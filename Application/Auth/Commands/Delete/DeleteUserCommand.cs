using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Commands.Delete;

public sealed record DeleteUserCommand(Guid id) : ICommand<User>;
