using Application.Permissions.Queries.GetAll;
using Domain.Permissions;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Threading;
using WebAPI.Helpers;

namespace WebAPI.Endpoints
{
    public static class PermissionsEndpoints
    {
        public static void MapPermissionsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app
                .MapGroup("permissions")
                .WithTags("Permissions");

            group.MapGet("",
                [HasPermission(PermissionConstants.PermissionsRead)]
                async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetAllPermissionsQuery(), cancellationToken);
                if (result.IsFailure)
                {
                    HandleFailure(result);
                }

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(List<PermissionDetails>));
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
                
                { Error: { Code: "PermissionsNotFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Permissions Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),
                
                { Error: { Code: "OperationFailed" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Permission Name Already Exists",
                    StatusCodes.Status400BadRequest,
                    result.Error)),

                _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Internal server error",
                    StatusCodes.Status500InternalServerError,
                    result.Error))
            };
    }
}
