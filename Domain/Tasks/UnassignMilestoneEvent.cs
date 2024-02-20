using Domain.Milestones;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class UnassignMilestoneEvent: Event
{
    public Guid? MilestoneId { get; set; }
    public Milestone? Milestone { get; set; }

    public UnassignMilestoneEvent() {}
    public UnassignMilestoneEvent(string title, Guid creatorId, Guid taskId, Guid milestoneId) 
        : base(title, EventType.MILESTONE_UNASSIGNED, creatorId, taskId)
    {
        MilestoneId = milestoneId;
    }
}