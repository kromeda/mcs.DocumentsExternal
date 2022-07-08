using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Threading.Tasks;

namespace DocumentsExternal.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        private static readonly int exceptionId = (int)LogType.ControllerException;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger) =>
            (this.next, this.logger) = (next, logger);

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Reason = "Ошибка сервиса.", Description = "При выполнении запроса вызникла ошибка." });

                logger.LogError(exceptionId, ex, "Ошибка сервиса.");
            }
        }
    }
}
