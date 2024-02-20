using Application.Shared;

namespace Application.Repositories.Commands.Create.CreateForUser;

public sealed record CreateRepositoryForUserCommand(string Name, string Description,
    bool IsPrivate, Guid CreatorId) : ICommand<Guid>;