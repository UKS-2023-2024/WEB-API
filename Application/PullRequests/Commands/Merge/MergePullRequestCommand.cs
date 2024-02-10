using Application.Shared;
using Domain.Tasks.Enums;

namespace Application.PullRequests.Commands.Merge;

public record MergePullRequestCommand(Guid PullRequestId, Guid RepositoryId, Guid UserId,MergeType MergeType) : ICommand;