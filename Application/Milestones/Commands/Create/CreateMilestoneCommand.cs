using Application.Shared;

namespace Application.Milestones.Commands.Create;

public sealed record CreateMilestoneCommand(string RepositoryId,
    string Title, string Description, Guid CreatorId, DateOnly? DueDate) : ICommand<Guid>;