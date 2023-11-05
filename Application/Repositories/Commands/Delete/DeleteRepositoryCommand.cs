using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.Delete;

public record DeleteRepositoryCommand(Guid userId, Guid repositoryId): ICommand;