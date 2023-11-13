using Application.Shared;

namespace Application.Organizations.Commands.AcceptInvite;

public record AcceptInviteCommand(Guid Authorized, Guid InviteId) : ICommand;
