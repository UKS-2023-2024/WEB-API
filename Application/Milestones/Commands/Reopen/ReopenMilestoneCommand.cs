using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Commands.Reopen;

public record ReopenMilestoneCommand(Guid UserId, Guid MilestoneId): ICommand<Milestone>;