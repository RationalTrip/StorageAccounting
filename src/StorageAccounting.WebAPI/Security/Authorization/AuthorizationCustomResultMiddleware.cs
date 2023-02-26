using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using StorageAccounting.Application.Models.Dtos;
using System.Text.Json;

namespace StorageAccounting.WebAPI.Security.Authorization
{
    internal class AuthorizationCustomResultMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (!authorizeResult.Succeeded)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                context.Response.ContentType = "application/json";

                await context.Response
                    .WriteAsync(JsonSerializer.Serialize(UnauthorizedResult));

                return;
            }

            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }

        private ErrorDto UnauthorizedResult => new ErrorDto
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Message = "Authorization failed."
        };
    }
}
