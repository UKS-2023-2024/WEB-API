using Application.Shared;

namespace Application.Milestones.Commands.Reopen;

public record ReopenMilestoneCommand(Guid UserId, Guid MilestoneId): ICommand<Guid>;