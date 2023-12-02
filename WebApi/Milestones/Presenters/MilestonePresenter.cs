using Domain.Milestones;

namespace WEB_API.Milestones.Presenters;

public class MilestonePresenter
{
    public Guid Id { get;  set; }
    public string Title { get;  set; }
    public DateOnly? DueDate { get;  set; }
    public string Description { get;  set; }
    public Guid RepositoryId { get;  set; }

    public MilestonePresenter(Milestone milestone)
    {
        Id = milestone.Id;
        Title = milestone.Title;
        DueDate = milestone.DueDate;
        Description = milestone.Description;
        RepositoryId = milestone.RepositoryId;
    }

    public static List<MilestonePresenter> MapFromMilestonesToMilestonePresenters(List<Milestone> milestones)
    {
        List<MilestonePresenter> presenters = new List<MilestonePresenter>();
        foreach (Milestone m in milestones)
        {
            presenters.Add(new MilestonePresenter(m));
        }

        return presenters;
    }
}