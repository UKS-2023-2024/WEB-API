using Application.Shared;
using Domain.Auth;

namespace Application.Organizations.Commands.Delete;

public record DeleteOrganizationCommand(Guid OrganizationId, Guid UserId): ICommand;