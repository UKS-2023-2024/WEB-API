using Application.Shared;

namespace Application.Milestones.Commands.Delete;

public record DeleteMilestoneCommand(Guid UserId, Guid MilestoneId) : ICommand<Guid>;