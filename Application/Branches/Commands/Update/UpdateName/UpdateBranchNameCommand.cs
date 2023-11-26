using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Commands.Update;

public sealed record UpdateBranchNameCommand(Guid BranchId, string Name): ICommand<Branch>;