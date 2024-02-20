using Application.Shared;
using Domain.Organizations;

namespace Application.Organizations.Queries.FindUserOrganizations;

public record FindUserOrganizationsQuery(Guid UserId) : IQuery<List<Organization>>;