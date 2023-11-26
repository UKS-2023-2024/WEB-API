using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Commands.Restore;

public sealed record RestoreBranchCommand(Guid BranchId) : ICommand<Branch>;