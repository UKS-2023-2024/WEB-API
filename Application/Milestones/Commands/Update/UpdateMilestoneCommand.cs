using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Commands.Update;

public sealed record UpdateMilestoneCommand(Guid MilestoneId,
    string Title, string Description, Guid CreatorId, DateOnly DueDate): ICommand<Milestone>;