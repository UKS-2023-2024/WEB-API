using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Commands.Delete;

public sealed record DeleteBranchCommand(Guid BranchId) : ICommand<Branch>;