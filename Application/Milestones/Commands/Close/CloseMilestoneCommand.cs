using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Commands.Close;

public record CloseMilestoneCommand(Guid UserId, Guid MilestoneId) : ICommand<Milestone>;