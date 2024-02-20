using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using MediatR;

namespace Application.Milestones.Queries.FindCompletionPercentageOfMilestone;

public class FindCompletionPercentageOfMilestoneQueryHandler : IRequestHandler<FindCompletionPercentageOfMilestoneQuery, double>
{
    private readonly IIssueRepository _issueRepository;

    public FindCompletionPercentageOfMilestoneQueryHandler(IIssueRepository issueRepository)
    {
        _issueRepository = issueRepository;
    }
    public async Task<double> Handle(FindCompletionPercentageOfMilestoneQuery request, CancellationToken cancellationToken)
    {
        List<Issue> issues = _issueRepository.FindMilestoneIssues(request.MilestoneId).Result;
        if (issues.Count > 0)
        {
            double completionPercentage = ((double)issues.Count(i => i.State == TaskState.CLOSED) / issues.Count) * 100;
            return completionPercentage;
        }
           
        return 0;
    }
}