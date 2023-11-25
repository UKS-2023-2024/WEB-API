using Application.Shared;
using Domain.Repositories;

namespace Application.Repositories.Commands.HandleRepositoryMembers.ChangeRole;

public sealed record  ChangeMemberRoleCommand(Guid OwnerId, Guid RepositoryMemberId, Guid RepositoryId, RepositoryMemberRole Role) : ICommand;