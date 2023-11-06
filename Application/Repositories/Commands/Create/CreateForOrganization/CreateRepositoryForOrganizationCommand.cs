using Application.Shared;

namespace Application.Repositories.Commands.Create.CreateForOrganization;

public sealed record CreateRepositoryForOrganizationCommand(string Name, string Description,
    bool IsPrivate, Guid CreatorId, Guid OrganizationId) : ICommand<Guid>;