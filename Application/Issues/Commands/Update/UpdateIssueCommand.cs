using System.Windows.Input;
using Application.Issues.Commands.Enums;
using Application.Shared;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;

namespace Application.Issues.Commands.Update;

public record UpdateIssueCommand(Guid Id, Guid UserId, string Title, string Description, TaskState State, int Number,
    Guid RepositoryId, List<string> AssigneesIds, List<string> LabelsIds, UpdateIssueFlag Flag, Guid? MilestoneId): ICommand<Guid>;