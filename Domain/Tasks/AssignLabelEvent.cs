using Domain.Tasks.Enums;

namespace Domain.Tasks.Interfaces;

public class AssignLabelEvent: Event
{
    public Guid LabelId { get; set; }
    public Label Label { get; set; }
    
    public AssignLabelEvent() {}
    public AssignLabelEvent(string title, Guid creatorId, Guid taskId, Guid labelId) 
        : base(title, EventType.LABEL_ASSIGNED, creatorId, taskId)
    {
        LabelId = labelId;
    }
}