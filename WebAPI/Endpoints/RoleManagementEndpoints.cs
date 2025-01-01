using Application.Roles.Commands.Create;
using Application.Roles.Commands.Update;
using Application.Roles.Queries.GetAll;
using Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernal;
using System;
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
                .RequireAuthorization(policy => policy.RequireRole(Roles.Admin))
                .WithTags("Role Management");

            group.MapPost("create", async (
                CreateRoleRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new CreateRoleCommand(request.RoleName.ToLower()), cancellationToken);
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

            group.MapPut("update", async (
                UpdateRoleRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateRoleCommand(request.RoleId, request.RoleName.ToLower());
                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            });

            group.MapGet("", async (
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllRolesQuery(
                    searchTerm,
                    sortColumn,
                    sortOrder,
                    page,
                    pageSize);

                var result = await sender.Send(query, cancellationToken);

                return Results.Ok(result.Value);
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

                { Error: { Code: "RoleNotFound" } } =>
                Results.NotFound(ErrorHandler.CreateProblemDetails(
                    "Role Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                { Error: { Code: "RoleNameAlreadyExists" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Role Name Already Exists",
                    StatusCodes.Status409Conflict,
                    result.Error)),

                _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Internal server error",
                    StatusCodes.Status500InternalServerError,
                    result.Error))
            };
    }
}
