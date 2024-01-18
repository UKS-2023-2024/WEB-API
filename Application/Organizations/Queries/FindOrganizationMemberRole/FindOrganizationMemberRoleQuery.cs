using Application.Shared;

namespace Application.Organizations.Queries.FindOrganizationMemberRole;

public sealed record FindOrganizationMemberRoleQuery(Guid UserId, Guid OrganizationId) : IQuery<string?>;