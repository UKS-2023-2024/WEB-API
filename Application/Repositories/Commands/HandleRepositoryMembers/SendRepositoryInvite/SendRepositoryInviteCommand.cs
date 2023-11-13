using Application.Shared;

namespace Application.Repositories.Commands.HandleRepositoryMembers.SendRepositoryInvite;

public sealed record  SendRepositoryInviteCommand(Guid OwnerId, Guid UserId, Guid RepositoryId) : ICommand;