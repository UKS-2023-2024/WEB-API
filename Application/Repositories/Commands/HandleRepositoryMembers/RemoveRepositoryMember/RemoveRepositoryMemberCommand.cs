using Application.Shared;

namespace Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;

public sealed record  RemoveRepositoryMemberCommand(Guid OwnerId, Guid RepositoryMemberId, Guid RepositoryId) : ICommand;