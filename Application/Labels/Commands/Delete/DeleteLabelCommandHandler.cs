using Application.Shared;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Labels.Commands.Delete;

public class DeleteLabelCommandHandler: ICommandHandler<DeleteLabelCommand, Guid>
{
    private readonly ILabelRepository _labelRepository;
    private readonly IIssueRepository _issueRepository;

    public DeleteLabelCommandHandler(ILabelRepository labelRepository, IIssueRepository issueRepository)
    {
        _labelRepository = labelRepository;
        _issueRepository = issueRepository;
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
        label.Delete();
        _labelRepository.Update(label);
        return label.Id;
    }
}