using Application.Shared.Email;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Events;
using Domain.Organizations.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Organizations.Events.MemberInvited;

public class OrganizationMemberInvitedEventHandler: INotificationHandler<OrganizationMemberInvitedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IOrganizationInviteRepository _organizationInviteRepository;
    private readonly IUserRepository _userRepository;
    
    public OrganizationMemberInvitedEventHandler(
        IOrganizationInviteRepository organizationInviteRepository, 
        IConfiguration configuration,
        IEmailService emailService, 
        IUserRepository userRepository)
    {
        _emailService = emailService;
        _configuration = configuration;
        _organizationInviteRepository = organizationInviteRepository;
        _userRepository = userRepository;
    }
    
    public async Task Handle(OrganizationMemberInvitedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Event triggered for sending invitation email!");
        var invite = await _organizationInviteRepository.FindById(notification.InviteId);
        var user = await _userRepository.FindUserById(invite.UserId);
        User.ThrowIfDoesntExist(user);
        var link = $"{_configuration["PublicApp"]}/invites/{invite.Id}";
        await _emailService.SendOrgInvitationLink(user.PrimaryEmail, link);
    }
}