using Application.Shared;
using Domain.Organizations;

namespace Application.Organizations.Queries.FindOrganizationMemberRole;

public sealed record FindOrganizationMemberRoleQuery(Guid UserId, Guid OrganizationId) : IQuery<OrganizationMemberRole>;