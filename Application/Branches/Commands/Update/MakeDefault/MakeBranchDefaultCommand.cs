using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Commands.Update;

public sealed record MakeBranchDefaultCommand(Guid BranchId): ICommand<Branch>;