using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;

public sealed record RemoveRepositoryMemberCommand(Guid OwnerId, Guid UserId, Guid RepositoryId) : ICommand;