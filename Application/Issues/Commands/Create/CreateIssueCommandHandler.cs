﻿using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Commands.Create;

public class CreateIssueCommandHandler : ICommandHandler<CreateIssueCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    public CreateIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
        IRepositoryRepository repositoryRepository, IIssueRepository issueRepository,
        IUserRepository userRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        int taskNumber = await _taskRepository.GetTaskNumber() + 1;
        Repository repository = _repositoryRepository.Find(request.RepositoryId);
        User creator = _userRepository.Find(request.UserId);
        Issue issue = Issue.Create(request.Title, request.Description, TaskState.OPEN,
            taskNumber,
            repository, creator);
        Issue createdIssue = await _issueRepository.Create(issue);
        return createdIssue.Id;
    }
}