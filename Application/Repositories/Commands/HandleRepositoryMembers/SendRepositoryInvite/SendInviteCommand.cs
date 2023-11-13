using Application.Shared;

namespace Application.Repositories.Commands.HandleRepositoryMembers.SendRepositoryInvite;

public sealed record  SendInviteCommand(Guid OwnerId, Guid UserId, Guid RepositoryId) : ICommand;