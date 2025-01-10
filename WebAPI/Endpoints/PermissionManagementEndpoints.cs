using Application.Permissions.Commands.Create;
using Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernal;
using System;
using System.Security.Claims;
using System.Threading;
using WebAPI.Helpers;

namespace WebAPI.Endpoints
{
    public static class PermissionManagementEndpoints
    {
        public static void MapPermissionManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app
                .MapGroup("permissions")
                .RequireAuthorization(policy => policy.RequireRole(Roles.Admin))
                .WithTags("Permission Management");

            group.MapPost("create", async (
                CreatePermissionRequest request,
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
                var command = new CreatePermissionCommand(
                    request.Name.Trim().ToLower(), 
                    string.IsNullOrWhiteSpace(request.Description) 
                        ? null
                        : request.Description, 
                    userId);

                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess
                    ? Results.Created(uri: $"/roles/{result.Value}",
                    value: new
                    {
                        Message = "Permission created successfully."
                    })
                    : HandleFailure(result);
            })
                .Produces(StatusCodes.Status201Created);
        }

        private static IResult HandleFailure(Result result) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),

                { Error: { Code: "ValidationError" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Validation Errors",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    result.Error.Details.ToArray())),

                { Error: { Code: "IdentityError" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Operation Failed",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    result.Error.Details.ToArray())),

                { Error: { Code: "PermissionNotFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Permission Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                { Error: { Code: "PermissionNameAlreadyExists" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Permission Name Already Exists",
                    StatusCodes.Status409Conflict,
                    result.Error)),

                _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Internal server error",
                    StatusCodes.Status500InternalServerError,
                    result.Error))
            };
    }
}
