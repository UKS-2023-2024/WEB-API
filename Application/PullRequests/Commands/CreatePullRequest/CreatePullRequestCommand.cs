using Application.Shared;

namespace Application.PullRequests.Commands;

public record CreatePullRequestCommand(Guid UserId, string Title, string? Description,
    Guid RepositoryId, List<Guid>? AssigneesIds, List<Guid>? LabelsIds, Guid? MilestoneId,
    Guid FromBranchId, Guid ToBranchId, List<Guid>? issueIds): ICommand<Guid>;