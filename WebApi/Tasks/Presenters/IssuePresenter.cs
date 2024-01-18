using System.ComponentModel.DataAnnotations;
using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Presenters;

public class IssuePresenter
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RepositoryId { get; set; }
    public List<RepositoryMember> Assignees { get; set; }
    public List<Label> Labels { get; set; }
    public Milestone Milestone { get; set; }
    public int Number { get; set; }
    public TaskState State { get; set; }

    public List<Event> Events { get; set; }

    public IssuePresenter(Issue issue)
    {
        Id = issue.Id.ToString();
        Title = issue.Title;
        Description = issue.Description;
        RepositoryId = issue.RepositoryId.ToString();
        Assignees = issue.Assignees;
        Labels = issue.Labels;
        Milestone = issue.Milestone;
        Number = issue.Number;
        Events = issue.Events;
        State = issue.State;
    }

    public static List<IssuePresenter> MapIssueToIssuePresenter(List<Issue> issues)
    {
        List<IssuePresenter> issuePresenters = new List<IssuePresenter>();
        foreach (Issue issue in issues)
        {
            issuePresenters.Add(new IssuePresenter(issue));
        }
        return issuePresenters;
    }

}