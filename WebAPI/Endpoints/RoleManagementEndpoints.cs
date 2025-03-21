﻿using Application.Roles.Commands.Activate;
using Application.Roles.Commands.AssignPermissions;
using Application.Roles.Commands.Create;
using Application.Roles.Commands.Deactivate;
using Application.Roles.Commands.Delete;
using Application.Roles.Commands.Update;
using Application.Roles.Queries.GetAll;
using Application.Roles.Queries.GetRoleNames;
using Application.Roles.Queries.GetRolePermissions;
using Application.Shared.Pagination;
using Application.Shared.Requesets;
using Domain.Permissions;
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
    public static class RoleManagementEndpoints
    {
        public static void MapRoleManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app
                .MapGroup("roles")
                .WithTags("Role Management");

            group.MapPost("create", 
                [HasPermission(SsoPermissionConstants.RolesCreate)] 
                async (
                    CreateRoleRequest request,
                    HttpContext httpContext,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
                var command = new CreateRoleCommand(request.RoleName.Trim().ToLower(), request.Description, request.AppId, userId);
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Created(
                    uri: $"/roles/{result.Value}",
                    value: new
                    {
                        Message = "Role created successfully."
                    });
            })
                .Produces(StatusCodes.Status201Created);

            group.MapPut("update", 
                [HasPermission(SsoPermissionConstants.RolesEdit)] 
                async (
                    UpdateRoleRequest request,
                    HttpContext httpContext,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new UpdateRoleCommand(
                    request.RoleId,
                    request.RoleName.Trim().ToLower(),
                    request.Description,
                    userId);

                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent);

            group.MapGet("", 
                [HasPermission(SsoPermissionConstants.RolesAccess)] 
                async (
                    string? searchTerm,
                    string? sortColumn,
                    string? sortOrder,
                    int page,
                    int pageSize,
                    int appId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var query = new GetAllRolesQuery(
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize,
                    appId);

                var result = await sender.Send(query, cancellationToken);

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(PaginatedList<GetAllRolesResponse>));

            group.MapGet("role-names",
                [HasPermission(SsoPermissionConstants.RolesAccess)]
                async (
                    int appId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetRoleNamesQuery(appId), cancellationToken);

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(string[]));

            group.MapPut("permissions", 
                [HasPermission(SsoPermissionConstants.RolesPermissionsManage)] 
                async (
                    AssignRolePermissionsRequest request,
                    HttpContext httpContext,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new AssignRolePermissionsCommand(request.RoleId, request.PermissionIds, userId);
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent);

            group.MapGet("permissions", 
                [HasPermission(SsoPermissionConstants.RolesPermissionsAccess)] 
                async (
                    string roleId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var query = new GetRolePermissionsQuery(roleId);
                var result = await sender.Send(query, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(RolePermissionsResponse));

            group.MapPut("activate", 
                [HasPermission(SsoPermissionConstants.RolesStatusChange)] 
                async (
                    BulkIdsRequest<string> ids,
                    HttpContext httpContext,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new ActivateRolesCommand(ids.Ids.ToList(), userId);
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                var activatedIds = result.Value;

                if (activatedIds.Count == 1)
                {
                    return Results.Ok(
                    new
                    {
                        Message = $"Role, {activatedIds[0]} was activated."
                    });
                }

                return Results.Ok(
                    new
                    {
                        Message = $"Roles, {string.Join(", ", activatedIds)} were activated."
                    });
            })
                .Produces(StatusCodes.Status200OK);

            group.MapPut("deactivate", 
                [HasPermission(SsoPermissionConstants.RolesStatusChange)] 
                async (
                    BulkIdsRequest<string> ids,
                    HttpContext httpContext,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new DeactivateRolesCommand(ids.Ids.ToList(), userId);
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
                        Message = $"Role with id, {deactivatedIds[0]} was deactivated."
                    });
                }

                return Results.Ok(
                    new
                    {
                        Message = $"Roles with ids, {string.Join(",", deactivatedIds)} were deactivated."
                    });
            })
                .Produces(StatusCodes.Status200OK);
            
            group.MapPut("delete", 
                [HasPermission(SsoPermissionConstants.RolesDelete)] 
                async (
                    BulkIdsRequest<string> ids,
                    HttpContext httpContext,
                    ISender sender,
                    CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var command = new DeleteRolesCommand(ids.Ids.ToList(), userId);
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
                        Message = $"Role with id, {deletedIds[0]} was deleted."
                    });
                }

                return Results.Ok(
                    new
                    {
                        Message = $"Roles with ids, {string.Join(",", deletedIds)} were deleted."
                    });
            })
                .Produces(StatusCodes.Status200OK);

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

                { Error: { Code: "RoleNotFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Role Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),
                
                { Error: { Code: "RolesNotFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Roles Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),
                
                { Error: { Code: "UserRolesNotFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "User Roles Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                { Error: { Code: "RoleNameAlreadyExists" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Role Name Already Exists",
                    StatusCodes.Status409Conflict,
                    result.Error)),
                
                { Error: { Code: "MissingPermissions" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Missing Permissions",
                    StatusCodes.Status404NotFound,
                    result.Error)),
                
                { Error: { Code: "NoPermissionsFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "No Permissions Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Internal server error",
                    StatusCodes.Status500InternalServerError,
                    result.Error))
            };
    }
}
