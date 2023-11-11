using Application.Shared.Email;
using Domain.Organizations;
using Domain.Organizations.Events;
using Domain.Organizations.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Organizations.Events.MemberInvited;

public class OrganizationMemberInvitedEventHandler: INotificationHandler<OrganizationMemberInvitedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IOrganizationMemberRepository _memberRepository;
    private readonly IConfiguration _configuration;
    
    public OrganizationMemberInvitedEventHandler(IConfiguration configuration,IEmailService emailService, IOrganizationMemberRepository memberRepository)
    {
        _emailService = emailService;
        _memberRepository = memberRepository;
        _configuration = configuration;
    }
    
    public async Task Handle(OrganizationMemberInvitedEvent notification, CancellationToken cancellationToken)
    {
        var memberId = notification.Invite.MemberId;
        var organizationId = notification.Invite.OrganizationId;
        var member = await this._memberRepository.FindPopulated(organizationId, memberId);
        OrganizationMember.ThrowIfDoesntExist(member);
        var email = member.Member.PrimaryEmail;

        var link = _configuration["PublicAppUrl"] + "/invite/" + notification.Invite.Token;
        _emailService.SendOrgInvitationLink(email, link);
    }
}