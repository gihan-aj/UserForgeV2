using Application.Shared.Pagination;
using Application.Shared.Requesets;
using Application.UserManagement.Commands.Activate;
using Application.UserManagement.Commands.AssignRoles;
using Application.UserManagement.Commands.Deactivate;
using Application.UserManagement.Commands.Delete;
using Application.UserManagement.Queries.GetAll;
using Domain.Permissions;
using Domain.Roles;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernal;
using System;
using System.Linq;
using System.Security.Claims;
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
                .RequireAuthorization(policy => policy.RequireRole(RoleConstants.Admin))
                .WithTags("User Management");

            group.MapGet("",
                [HasPermission(PermissionConstants.UsersRead)] 
                async (
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
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new ActivateUsersCommand(request.Ids.ToList(), userId);
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
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new DeactivateUsersCommand(request.Ids.ToList(), userId);
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

            group.MapPut("assign-roles", async (
                AssignUserRolesRequest request,
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
                var command = new AssignUserRolesCommand(request.UserId, request.RoleNames, userId);
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }
                var assignedRoleIds = result.Value;
                if(assignedRoleIds.Count == 0)
                {
                    return Results.Ok(
                    new
                    {
                        Message = $"No roles were assigned to user."
                    });
                }
                if (assignedRoleIds.Count == 1)
                {
                    return Results.Ok(
                    new
                    {
                        Message = $"Role, {assignedRoleIds[0]} was assigned to user."
                    });
                }
                return Results.Ok(
                    new
                    {
                        Message = $"Roles, {string.Join(",", assignedRoleIds)} were assigned to user."
                    });
            });

            group.MapPut("delete", async (
                BulkIdsRequest<string> request,
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new DeleteUsersCommand(request.Ids.ToList(), userId);
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }
                var deletedIds = result.Value;
                if (deletedIds.Count == 1)
                {
                    return Results.Ok(
                    new
                    {
                        Message = $"User with id, {deletedIds[0]} was deleted."
                    });
                }
                return Results.Ok(
                    new
                    {
                        Message = $"Users with ids, {string.Join(",", deletedIds)} were deleted."
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
                    "Operation Failed",
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
