using Application.Shared;
using Domain.Organizations;

namespace Application.Organizations.Commands.ChangeOrganizationMemberRole;

public sealed record  ChangeOrganizationMemberRoleCommand(Guid OwnerId, Guid MemberId, Guid OrganizationId, OrganizationMemberRole Role) : ICommand;