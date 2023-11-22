using Domain.Exceptions;

namespace Domain.Milestones.Exceptions;

public class MilestoneNotFoundException : BaseException
{
    public MilestoneNotFoundException() : base("Milestone not found!")
    {
    }
}