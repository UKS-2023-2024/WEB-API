using System.Net;
using Domain.Auth.Exceptions;
using Domain.Exceptions;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;
namespace WEB_API.ExceptionHandling;

public static class ExceptionHandler
{
    public static int GetStatusCode(this BaseException exception)
    {
        HttpStatusCode code = exception switch
        {
            InvalidCredentialsException or UnautorizedAccessException => HttpStatusCode.Unauthorized,
            UserNotFoundException or RepositoryNotFoundException or OrganizationNotFoundException => HttpStatusCode.NotFound,
            UserWithThisEmailExistsException or RepositoryWithThisNameExistsException => HttpStatusCode.Conflict,
            PermissionDeniedException => HttpStatusCode.Forbidden,

        };

        return (int)code;
    }
}