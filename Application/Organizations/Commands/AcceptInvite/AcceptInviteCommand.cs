using Application.Shared;

namespace Application.Organizations.Commands.AcceptInvite;

public record AcceptInviteCommand(Guid InviteId) : ICommand;
