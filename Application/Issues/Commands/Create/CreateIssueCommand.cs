using System.Windows.Input;
using Application.Shared;
using Domain.Auth;
using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks;

namespace Application.Issues.Commands.Create;

public record CreateIssueCommand(Guid UserId, string Title, string Description,
    Guid RepositoryId, List<RepositoryMember> Assignees, List<Label> Labels, Guid? MilestoneId): ICommand<Guid>;