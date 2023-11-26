using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Events;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Repositories.Events.MemberInvited;

public class RepositoryMemberInvitedEventHandler: INotificationHandler<RepositoryMemberInvitedEvent>
{
    
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IRepositoryInviteRepository _repositoryInviteRepository;
    private readonly IUserRepository _userRepository;
    
    public RepositoryMemberInvitedEventHandler(
        IRepositoryInviteRepository repositoryInviteRepository, 
        IConfiguration configuration,
        IEmailService emailService, 
        IUserRepository userRepository)
    {
        _emailService = emailService;
        _configuration = configuration;
        _repositoryInviteRepository = repositoryInviteRepository;
        _userRepository = userRepository;
    }
    
    public async Task Handle(RepositoryMemberInvitedEvent notification, CancellationToken cancellationToken)
    {
        var invite = await _repositoryInviteRepository.FindById(notification.InviteId);
        RepositoryInvite.ThrowIfDoesntExist(invite);
        var user = await _userRepository.FindUserById(invite.UserId);
        User.ThrowIfDoesntExist(user);
        var link = $"{_configuration["PublicApp"]}/repository/invites/{invite.Id}";
        await _emailService.SendRepoInvitationLink(user.PrimaryEmail, link,invite.Repository.Name);
    }
}