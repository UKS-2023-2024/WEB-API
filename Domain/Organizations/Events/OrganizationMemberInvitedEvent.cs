using MediatR;

namespace Domain.Organizations.Events;

public record OrganizationMemberInvitedEvent(Guid InviteId): INotification;
