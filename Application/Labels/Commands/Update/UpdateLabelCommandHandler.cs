using Application.Shared;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Labels.Commands.Update;

public class UpdateLabelCommandHandler: ICommandHandler<UpdateLabelCommand, Label>
{
    private readonly ILabelRepository _labelRepository;

    public UpdateLabelCommandHandler(ILabelRepository labelRepository)
    {
        _labelRepository = labelRepository;
    }

    public async Task<Label> Handle(UpdateLabelCommand request, CancellationToken cancellationToken)
    {
        Label foundLabel = _labelRepository.Find(request.Id);
        Label updatedLabel = foundLabel.Update(request.Title, request.Description, request.Color);
        _labelRepository.Update(updatedLabel);
        return updatedLabel;
    }
}