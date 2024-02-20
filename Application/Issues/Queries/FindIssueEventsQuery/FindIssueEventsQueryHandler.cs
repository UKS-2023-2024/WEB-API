using Application.Shared;
using Domain.Milestones.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using FluentResults;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace Application.Issues.Queries.FindIssueEventsQuery;

public class FindIssueEventsQueryHandler: IQueryHandler<FindIssueEventsQuery, List<Event>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ILabelRepository _labelRepository;

    
    public FindIssueEventsQueryHandler(IIssueRepository issueRepository, IMilestoneRepository milestoneRepository,
        IRepositoryMemberRepository repositoryMemberRepository, ILabelRepository labelRepository)
    {
        _issueRepository = issueRepository;
        _milestoneRepository = milestoneRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _labelRepository = labelRepository;
    }

    public async Task<List<Event>> Handle(FindIssueEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _issueRepository.FindOpenedIssueEvents(request.Id);
        events = events.Select(e =>
        {
            if (e.EventType == EventType.ISSUE_ASSIGNED)
            {
                var ev = (AssignEvent)e;
                var assignee = _repositoryMemberRepository.Find(ev.AssigneeId);
                ev.Assignee = assignee;
                return ev;
            }
            
            if (e.EventType == EventType.ISSUE_UNASSIGNED)
            {
                var ev = (UnassignEvent)e;
                var assignee = _repositoryMemberRepository.Find(ev.AssigneeId);
                ev.Assignee = assignee;
                return ev;
            }

            if (e.EventType == EventType.MILESTONE_ASSIGNED)
            {
                var ev = (AssignMilestoneEvent)e;
                var milestone = _milestoneRepository.Find(ev.MilestoneId);
                ev.Milestone = milestone;
                return ev;
            }
            
            if (e.EventType == EventType.MILESTONE_UNASSIGNED)
            {
                var ev = (UnassignMilestoneEvent)e;
                var milestone = _milestoneRepository.Find(ev.MilestoneId ?? new Guid());
                ev.Milestone = milestone;
                return ev;
            }
            
            if (e.EventType == EventType.LABEL_ASSIGNED)
            {
                var ev = (AssignLabelEvent)e;
                var label = _labelRepository.Find(ev.LabelId);
                ev.Label = label;
                return ev;
            }

            return e;
        }).ToList();
        
        return events;
    }
}