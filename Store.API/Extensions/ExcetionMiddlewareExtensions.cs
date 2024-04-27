using Microsoft.AspNetCore.Diagnostics;
using Store.Entities.ErrorModels;
using Store.Entities.Exceptions;
using Store.Service.Contracts;

namespace Store.API;

public static class ExcetionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app,ILoggerService logger)
    {
        app.UseExceptionHandler( appError => 
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    logger.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDetails(){
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message
                    }.ToString());
                }
            });
        } );
    }
}
