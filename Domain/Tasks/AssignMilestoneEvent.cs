using Domain.Milestones;
using Domain.Tasks.Enums;

namespace Domain.Tasks.Interfaces;

public class AssignMilestoneEvent: Event
{
    public Guid MilestoneId { get; set; }
    public Milestone? Milestone { get; set; }

    public AssignMilestoneEvent() {}
    public AssignMilestoneEvent(string title, Guid creatorId, Guid taskId, Guid milestoneId) 
        : base(title, EventType.MILESTONE_ASSIGNED, creatorId, taskId)
    {
        MilestoneId = milestoneId;
    }
}