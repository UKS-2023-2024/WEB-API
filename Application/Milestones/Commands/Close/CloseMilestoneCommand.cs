using Application.Shared;

namespace Application.Milestones.Commands.Close;

public record CloseMilestoneCommand(Guid UserId, Guid MilestoneId) : ICommand<Guid>;