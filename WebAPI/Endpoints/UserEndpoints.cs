using Application.Users.Commands.ConfirmEmail;
using Application.Users.Commands.Login;
using Application.Users.Commands.Refresh;
using Application.Users.Commands.Register;
using Application.Users.Commands.ResendEmailConfirmation;
using Application.Users.Queries.GetUser;
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
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app
                .MapGroup("user")
                .WithTags("User");

            group.MapPost("register", async (
                RegisterUserRequest req, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new RegisterUserCommand(
                    req.FirstName.ToLower(),
                    req.LastName.ToLower(),
                    req.Email.ToLower(),
                    string.IsNullOrWhiteSpace(req.PhoneNumber)? null : req.PhoneNumber,
                    req.DateOfBirth.HasValue? req.DateOfBirth : null,
                    req.Password);

                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Created(
                    uri : $"/users/{result.Value}",
                    value: new
                    {
                        Message = "User created successfully. Please check your email to confirm your account."
                    });
            })
                .Produces(StatusCodes.Status201Created)
                .AllowAnonymous();


            group.MapPut("confirm-email", async (
                string userId, 
                string token, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new ConfirmEmailCommand(userId, token), cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent)
                .AllowAnonymous();


            group.MapPost("resend-email-confirmation-link", async (
                string email, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new ResendEmailConfirmationCommand(email.ToLower()), cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            })
                .Produces(StatusCodes.Status204NoContent)
                .AllowAnonymous();


            group.MapPost("login", async (
                LoginUserRequest request, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new LoginUserCommand(
                    request.Email.ToLower(),
                    request.Password,
                    request.DeviceIdentifier);

                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(LoginUserResponse))
                .AllowAnonymous();

            group.MapPost("refresh", async (
                RefreshUserRequest request, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new RefreshUserCommand(request.RefreshToken, request.DeviceIdentifier);

                var result = await sender.Send(command, cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(RefreshUserResponse))
                .AllowAnonymous();

            group.MapGet("", async (
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var result = await sender.Send(new GetUserQuery(userId), cancellationToken);
                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Ok(result.Value);
            })
                .Produces(StatusCodes.Status200OK, typeof(GetUserResponse))
                .RequireAuthorization();

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

                { Error: { Code: "EmailAlreadyExists" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Email Already Exists",
                    StatusCodes.Status409Conflict,
                    result.Error)),

                { Error: { Code: "UserNotFound" } } =>
                Results.NotFound(ErrorHandler.CreateProblemDetails(
                    "User Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                { Error: { Code: "EmailNotFound" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Email Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

                { Error: { Code: "EmailAlreadyConfirmed" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Email Already Confirmed",
                    StatusCodes.Status409Conflict,
                    result.Error)),

                { Error: { Code: "EmailNotConfirmed" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Email Not Confirmed",
                    StatusCodes.Status400BadRequest,
                    result.Error)),

                { Error: { Code: "InvalidCredentials" } } =>
                Results.BadRequest(ErrorHandler.CreateProblemDetails(
                    "Invalid Credentials",
                    StatusCodes.Status400BadRequest,
                    result.Error)),

                { Error: { Code: "MissingRefreshToken" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Token Error",
                    StatusCodes.Status401Unauthorized,
                    result.Error)),

                { Error: { Code: "InvalidRefreshToken" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Token Error",
                    StatusCodes.Status401Unauthorized,
                    result.Error)),

                { Error: { Code: "InvalidAccessToken" } } =>
                Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Token Error",
                    StatusCodes.Status401Unauthorized,
                    result.Error)),

                { Error: { Code: "PasswordMismatch" } } =>
                Results.BadRequest(ErrorHandler.CreateProblemDetails(
                    "Password Mismatch",
                    StatusCodes.Status400BadRequest,
                    result.Error)),

                _ => Results.Problem(ErrorHandler.CreateProblemDetails(
                    "Internal server error",
                    StatusCodes.Status500InternalServerError,
                    result.Error))
            };
    }
}
