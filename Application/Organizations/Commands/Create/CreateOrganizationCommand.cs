using Application.Shared;
using Domain.Auth;

namespace Application.Organizations.Commands.Create;

public sealed record CreateOrganizationCommand(string Name,
    string ContactEmail, List<Guid> PendingMembers, Guid CreatorId) : ICommand<Guid>;