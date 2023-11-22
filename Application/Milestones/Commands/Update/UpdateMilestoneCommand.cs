using Application.Shared;

namespace Application.Milestones.Commands.Update;

public sealed record UpdateMilestoneCommand(Guid MilestoneId, Guid RepositoryId,
    string Title, string Description, Guid CreatorId, DateOnly DueDate): ICommand<Guid>;