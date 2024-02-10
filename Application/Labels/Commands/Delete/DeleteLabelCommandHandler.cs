using Application.Shared;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Labels.Commands.Delete;

public class DeleteLabelCommandHandler: ICommandHandler<DeleteLabelCommand, Guid>
{
    private readonly ILabelRepository _labelRepository;

    public DeleteLabelCommandHandler(ILabelRepository labelRepository)
    {
        _labelRepository = labelRepository;
    }

    public async Task<Guid> Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {
        Label label = _labelRepository.Find(request.Id);
        _labelRepository.Delete(label);
        return label.Id;
    }
}