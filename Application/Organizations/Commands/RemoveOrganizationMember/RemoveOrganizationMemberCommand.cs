using Application.Shared;

namespace Application.Organizations.Commands.RemoveOrganizationMember;

public sealed record  RemoveOrganizationMemberCommand(Guid OwnerId, Guid OrganizationMemberId, Guid OrganizationId) : ICommand;