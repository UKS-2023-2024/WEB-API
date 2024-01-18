using System.Net;
using Domain.Auth.Exceptions;
using Domain.Branches.Exceptions;
using Domain.Exceptions;
using Domain.Milestones.Exceptions;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;
using Domain.Tasks.Exceptions;

namespace WEB_API.ExceptionHandling;

public static class ExceptionHandler
{
    public static int GetStatusCode(this BaseException exception)
    {
        var code = exception switch
        {
            InvalidCredentialsException 
                or UnautorizedAccessException => HttpStatusCode.Unauthorized,
            UserNotFoundException 
                or RepositoryNotFoundException 
                or OrganizationNotFoundException
                or OrganizationInviteNotFoundException
                or OrganizationMemberNotFoundException
                or RepositoryMemberNotFoundException
                or RepositoryInviteNotFound
                or IssueNotFoundException
                or MilestoneNotFoundException
                or UserNotFoundException => HttpStatusCode.NotFound,
            UserWithThisEmailExistsException 
                or RepositoryWithThisNameExistsException
                or AlreadyOrganizationMemberException
                or BranchWithThisNameExistsException 
                or AlreadyRepositoryMemberException
                or BranchIsAlreadyDefaultException => HttpStatusCode.Conflict,
            PermissionDeniedException 
                or MemberHasNoPrivilegeException
                or RepositoryInaccessibleException
                or NotInviteOwnerException
                or UserNotAOrganizationMemberException => HttpStatusCode.Forbidden,
            InvitationExpiredException
                or RepositoryAlreadyStarredException
                or RepositoryMemberCantChangeHimselfException
                or RepositoryMemberCantBeDeletedException
                or CantRemoveOrganizationOwnerException
                or RepositoryNotStarredException => HttpStatusCode.BadRequest
        };
        return (int)code;
    }
}