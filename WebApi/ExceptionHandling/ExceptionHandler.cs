using System.Net;
using Domain.Auth.Exceptions;
using Domain.Exceptions;
using Domain.Organizations.Exceptions;

namespace WEB_API.ExceptionHandling;

public static class ExceptionHandler
{
    public static int GetStatusCode(this BaseException exception)
    {
        HttpStatusCode code = exception switch
        {
            InvalidCredentialsException => HttpStatusCode.Unauthorized,
            UserNotFoundException => HttpStatusCode.NotFound,
            UserWithThisEmailExistsException => HttpStatusCode.Conflict,
            PermissionDeniedException => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        };

        return (int)code;
    }
}