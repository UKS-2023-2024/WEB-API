using Application.Shared;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Labels.Commands.Delete;

public class DeleteLabelCommandHandler: ICommandHandler<DeleteLabelCommand, Guid>
{
    private readonly ILabelRepository _labelRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IPullRequestRepository _pullRequestRepository;

    public DeleteLabelCommandHandler(ILabelRepository labelRepository, IIssueRepository issueRepository, IPullRequestRepository pullRequestRepository)
    {
        _labelRepository = labelRepository;
        _issueRepository = issueRepository;
        _pullRequestRepository = pullRequestRepository;
    }

    public async Task<Guid> Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {
        Label label = _labelRepository.Find(request.Id);
        List<Issue> issues = await _issueRepository.FindAllAssignedWithLabelInRepository(label, label.RepositoryId);
        foreach (Issue issue in issues)
        {
            issue.Labels.Remove(label);
            _issueRepository.Update(issue);
        }
        List<PullRequest> pullRequests = await _pullRequestRepository.FindAllAssignedWithLabelInRepository(label, label.RepositoryId);
        foreach (PullRequest pr in pullRequests)
        {
            pr.Labels.Remove(label);
            _pullRequestRepository.Update(pr);
        }
        label.Delete();
        _labelRepository.Update(label);
        return label.Id;
    }
}