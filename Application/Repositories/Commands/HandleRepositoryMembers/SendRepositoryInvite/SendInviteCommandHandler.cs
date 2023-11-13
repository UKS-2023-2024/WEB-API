using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Events;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Commands.HandleRepositoryMembers.SendRepositoryInvite;

public class SendInviteCommandHandler: ICommandHandler<SendInviteCommand>
{
    private readonly IMediator _mediator;
    private readonly IRepositoryInviteRepository _repositoryInviteRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    public SendInviteCommandHandler(
        IMediator mediator,
        IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryInviteRepository repositoryInviteRepository,
        IUserRepository userRepository,
        IRepositoryRepository repositoryRepository, IOrganizationMemberRepository organizationMemberRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _mediator = mediator;
        _repositoryInviteRepository = repositoryInviteRepository;
        _userRepository = userRepository;
        _repositoryRepository = repositoryRepository;
        _organizationMemberRepository = organizationMemberRepository;
    }

    public async Task Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        var owner = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.OwnerId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(owner);
        owner!.ThrowIfNotOwner();

        var userToAdd = _userRepository.Find(request.UserId);
        User.ThrowIfDoesntExist(userToAdd);
        
        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (member is not null) 
            throw new AlreadyRepositoryMemberException();

        var existingInvitation = _repositoryInviteRepository.FindByRepoAndMember(request.RepositoryId, request.UserId);
        if (existingInvitation is not null)
            _repositoryInviteRepository.Delete(existingInvitation);

        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        if (repository!.Organization != null)
        {
            var organizationMember =
                await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.UserId, repository.Organization.Id);
            if (organizationMember == null) throw new UserNotAOrganizationMemberException();
        }

        var invite = RepositoryInvite.Create(request.UserId, request.RepositoryId);
        
        await _repositoryInviteRepository.Create(invite);
        await _mediator.Publish(new RepositoryMemberInvitedEvent(invite.Id), default);
    }
}