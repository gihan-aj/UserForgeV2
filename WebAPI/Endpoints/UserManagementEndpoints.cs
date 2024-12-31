using Application.Shared.Pagination;
using Application.Shared.Requesets;
using Application.Users.Commands.Activate;
using Application.Users.Commands.Deactivate;
using Application.Users.Queries.GetAll;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernal;
using System;
using System.Linq;
using System.Threading;
using WebAPI.Helpers;

namespace WebAPI.Endpoints
{
    public static class UserManagementEndpoints
    {
        public static void MapUserManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app
                .MapGroup("users")
                .RequireAuthorization(policy => policy.RequireRole(Roles.Admin))
                .WithTags("User Management");

            group.MapGet("", async (
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllUsersQuery(
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize);

                var result = await sender.Send(query, cancellationToken);

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(PaginatedList<GetAllUsersResponse>));

            group.MapPut("activate", async (
                BulkIdsRequest<string> request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ActivateUsersCommand(request.Ids.ToList());
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                var activatedIds = result.Value;

                if(activatedIds.Count == 1)
                {
                    return Results.Ok(
                    new
                    {
                        Message = $"User with id, {activatedIds[0]} was activated."
                    });
                }

                return Results.Ok(
                    new
                    {
                        Message = $"Users with ids, {string.Join(",", activatedIds)} were activated."
                    });
            });
            
            group.MapPut("deactivate", async (
                BulkIdsRequest<string> request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeactivateUsersCommand(request.Ids.ToList());
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                var deactivatedIds = result.Value;

                if (deactivatedIds.Count == 1)
                {
                    return Results.Ok(
                    new
                    {
                        Message = $"User with id, {deactivatedIds[0]} was deactivated."
                    });
                }

                return Results.Ok(
                    new
                    {
                        Message = $"Users with ids, {string.Join(",", deactivatedIds)} were deactivated."
                    });
            });
        }

        private static IResult HandleFailure(Result result) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),

                { Error: { Code: "ValidationError" } } =>
                Results.BadRequest(ErrorHandler.CreateProblemDetails(
                    "Validation Errors",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    result.Error.Details.ToArray())),

                { Error: { Code: "IdentityError" } } =>
                Results.BadRequest(ErrorHandler.CreateProblemDetails(
                    "Validation Errors",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    result.Error.Details.ToArray())),

                { Error: { Code: "UserNotFound" } } =>
                Results.NotFound(ErrorHandler.CreateProblemDetails(
                    "User Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),
                
                { Error: { Code: "UsersNotFound" } } =>
                Results.NotFound(ErrorHandler.CreateProblemDetails(
                    "Users Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                { Error: { Code: "InvalidAccessToken" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Token Error",
                    StatusCodes.Status401Unauthorized,
                    result.Error)),

                _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Internal server error",
                    StatusCodes.Status500InternalServerError,
                    result.Error))
            };
    }
}
