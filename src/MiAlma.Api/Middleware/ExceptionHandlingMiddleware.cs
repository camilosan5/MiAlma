using System.Net;
using System.Text.Json;
using MiAlma.Domain.Exceptions;

namespace MiAlma.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ex switch
                {
                    NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                    ForbiddenException => (HttpStatusCode.Forbidden, ex.Message),
                    UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message),
                    ConflictException => (HttpStatusCode.Conflict, ex.Message),
                    InvalidStatusTransitionException => (HttpStatusCode.BadRequest, ex.Message),
                    ProposalNotEditableException => (HttpStatusCode.BadRequest, ex.Message),
                    ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                    _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
                };

                if (statusCode == HttpStatusCode.InternalServerError)
                    _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var payload = JsonSerializer.Serialize(new { error = message });
                await context.Response.WriteAsync(payload);
            }
        }
    }
}
