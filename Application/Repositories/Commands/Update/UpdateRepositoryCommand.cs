using Application.Shared;
using Domain.Auth;
using Domain.Repositories;

namespace Application.Auth.Commands.Update;
public sealed record UpdateRepositoryCommand(Guid userId, Guid repositoryId, string Name, string Description, bool IsPrivate) : ICommand<Repository>;
