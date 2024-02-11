using Domain.Tasks;
using Domain.Tasks.Enums;
using Task = Domain.Tasks.Task;

namespace WEB_API.Tasks.Presenters;

public class IssuePullRequestPresenter
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid RepositoryId { get; set; }
    public int Number { get; set; }
    public TaskState State { get; set; }

    public IssuePullRequestPresenter(Task issue)
    {
        Id = issue.Id;
        Title = issue.Title;
        Description = issue.Description;
        RepositoryId = issue.RepositoryId;
        Number = issue.Number;
        State = issue.State;
    }
    public static List<IssuePullRequestPresenter> MapEventToEventPresenter(List<Issue> issues)
    {
        return issues.Select(issue => new IssuePullRequestPresenter(issue)).ToList();
    }
}