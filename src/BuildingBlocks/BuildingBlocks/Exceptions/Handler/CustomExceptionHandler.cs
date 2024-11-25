using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BuildingBlocks.Exceptions.Handler
{

    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            if (context?.Response == null || cancellationToken.IsCancellationRequested)
                return false;

            context.Response.ContentType = "application/json";

            logger.LogError("Error Message: {exceptionMessage}, Time: {time}",
                exception.Message, DateTime.UtcNow);

            (string Detail, string Title, int StatusCode) = exception switch
            {
                InternalServerException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status400BadRequest
                ),
                NoChangesMadeException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status200OK
                ),
                NotFoundException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status404NotFound
                ),
                _ =>
                (
                    exception.Message,
                    "UnexpectedError",
                    StatusCodes.Status500InternalServerError
                )
            };

            context.Response.StatusCode = StatusCode;

            var problemDetails = new ProblemDetails
            {
                Title = Title,
                Detail = Detail,
                Status = StatusCode,
                Instance = context.Request?.Path
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

            return true;
        }
    }

}
