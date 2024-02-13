using System.ComponentModel.DataAnnotations;
using Domain.Comments;
using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Presenters;

public class LabelPresenter
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }

    public LabelPresenter(Label label)
    {
        Id = label.Id.ToString();
        Title = label.Title;
        Description = label.Description;
        Color = label.Color;
    }

    public static List<LabelPresenter> MapLabelToLabelPresenter(List<Label> labels)
    {
        List<LabelPresenter> labelPresenters = new List<LabelPresenter>();
        foreach (Label label in labels)
        {
            labelPresenters.Add(new LabelPresenter(label));
        }
        return labelPresenters;
    }

}