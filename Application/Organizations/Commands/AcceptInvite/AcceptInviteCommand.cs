using Application.Shared;

namespace Application.Organizations.Commands.AcceptInvite;

public record AcceptInviteCommand(string Token) : ICommand;
