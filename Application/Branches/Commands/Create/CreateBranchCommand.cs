using Application.Shared;

namespace Application.Branches.Commands.Create;

public sealed record CreateBranchCommand(string Name, Guid RepositoryId, bool IsDefault) : ICommand<Guid>;