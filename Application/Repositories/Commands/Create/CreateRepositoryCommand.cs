using Application.Shared;

namespace Application.Repositories.Commands.Create;

public sealed record CreateRepositoryCommand(string Name, string Description, 
    bool IsPrivate, Guid CreatorId, Guid OrganizationId) : ICommand<Guid>;