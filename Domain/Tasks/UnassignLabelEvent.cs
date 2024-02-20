using Domain.Tasks.Enums;

namespace Domain.Tasks.Interfaces;

public class UnassignLabelEvent: Event
{
    public Guid LabelId { get; set; }
    public Label Label { get; set; }
    
    public UnassignLabelEvent() {}
    public UnassignLabelEvent(string title, Guid creatorId, Guid taskId, Guid labelId) 
        : base(title, EventType.LABEL_UNASSIGNED, creatorId, taskId)
    {
        LabelId = labelId;
    }
}