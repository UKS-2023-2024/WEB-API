using MediatR;

namespace Domain.Organizations.Events;

public record OrganizationMemberInvitedEvent(OrganizationInvite Invite): INotification;
