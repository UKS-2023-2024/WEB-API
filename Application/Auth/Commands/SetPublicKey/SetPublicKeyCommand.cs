using Application.Shared;

namespace Application.Auth.Commands.SetPublicKey;

public sealed record SetPublicKeyCommand(Guid UserId, string PublicKey): ICommand;