using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Task = Domain.Tasks.Task;

namespace Application.Labels.Create;

public class CreateLabelCommandHandler : ICommandHandler<CreateLabelCommand, Guid>
{
    private readonly ILabelRepository _labelRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;

    public CreateLabelCommandHandler(ILabelRepository labelRepository, IRepositoryMemberRepository repositoryMemberRepository,
        ITaskRepository taskRepository)
    {
        _labelRepository = labelRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
    }

    public async Task<Guid> Handle(CreateLabelCommand request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.CreatorId, request.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        Label label = Label.Create(request.Title, request.Description, request.Color, request.RepositoryId, request.isDefaultLabel);
        // Task task = _taskRepository.Find(request.TaskId);
        // label.Tasks.Add(task);
        await _labelRepository.Create(label);
        return label.Id;
    }
}