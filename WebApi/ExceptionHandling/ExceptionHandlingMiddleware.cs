using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace WEB_API.ExceptionHandling;

public static class ExceptionHandlingMiddleware
{
    public static void ConfigureGlobalErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(
            options =>
            {
                options.Run(
                    async context =>
                    {
                        Console.WriteLine("Triggered exception context");
                        int statusCode = (int)HttpStatusCode.InternalServerError;
                        string message = "";
                        var exception = context.Features.Get<IExceptionHandlerFeature>();

                        if (exception == null) return;
                                    
                        if (exception.Error.GetType().IsSubclassOf(typeof(BaseException)))
                        {
                            BaseException ex = (BaseException)exception.Error;
                            message = ex.Message;
                            statusCode = ex.GetStatusCode();
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = statusCode;
                                
                        var response = new ExceptionMessagePresenter(message, statusCode);
                        await context.Response.WriteAsync( response.ToString() );
                        await context.Response.CompleteAsync();

                    }
                );
            }
        );
    }
}