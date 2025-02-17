using Application.Apps.Commands.Create;
using Application.Apps.Commands.Update;
using Application.Apps.Queries.GetPaginated;
using Application.Shared.Pagination;
using Domain.Permissions;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernal;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using WebAPI.Helpers;

namespace WebAPI.Endpoints
{
    public static class AppManagementEndpoints
    {
        public static void MapAppManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("apps").WithTags("Apps");

            group.MapPost("create",
                [HasPermission(SsoPermissionConstants.AppsCreate)]
                async (
                CreateAppRequest request,
                HttpContext context,
                ISender sender,
                CancellationToken cancellationToken) => 
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new CreateAppCommand(
                    request.Name.ToLower().Trim(),
                    request.Description,
                    request.BaseUrl,
                    userId);

                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Created(
                    uri: $"/apps/{result.Value}",
                    value: new
                    {
                        Message = "App created successfully."
                    });
            })
                .Produces(StatusCodes.Status201Created);

            group.MapGet("", async (
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPaginatedAppListQuery(
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize);

                var result = await sender.Send(query, cancellationToken);

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(PaginatedList<PaginatedAppResponse>));

            group.MapPut("update",
                [HasPermission(SsoPermissionConstants.AppsEdit)]
                async (
                UpdateAppRequest request,
                HttpContext context,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new UpdateAppCommand(
                    request.Id,
                    request.Name.Trim().ToLower(),
                    request.Description,
                    request.BaseUrl,
                    userId);

                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent);
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

               { Error: { Code: "AppIdNotFound" } } =>
               Results.Problem(ErrorHandler.CreateProblemDetails(
                   "App Id Not Found",
                   StatusCodes.Status404NotFound,
                   result.Error)),

               { Error: { Code: "AppNotFound" } } =>
               Results.Problem(ErrorHandler.CreateProblemDetails(
                   "App Not Found",
                   StatusCodes.Status404NotFound,
                   result.Error)),

               { Error: { Code: "AppNameAlreadyExists" } } =>
               Results.Problem(ErrorHandler.CreateProblemDetails(
                   "App Name Already Exists",
                   StatusCodes.Status409Conflict,
                   result.Error)),

               _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                   "Internal server error",
                   StatusCodes.Status500InternalServerError,
                   result.Error))
           };
    }
}
