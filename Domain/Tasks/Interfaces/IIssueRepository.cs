using Domain.Shared.Interfaces;

namespace Domain.Tasks.Interfaces;

public interface IIssueRepository: IBaseRepository<Issue>
{
    Task<Issue> FindById(Guid id);
    Task<List<Issue>> FindRepositoryIssues(Guid repositoryId);
    Task<List<Event>> FindOpenedIssueEvents(Guid issueId);
    Task<List<Issue>> FindMilestoneIssues(Guid milestoneId);

}