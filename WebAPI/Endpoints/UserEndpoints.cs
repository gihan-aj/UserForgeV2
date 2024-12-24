using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace WebAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app
                .MapGroup("user")
                .WithTags("User");

            group.MapGet("", (CancellationToken cancellationToken) =>
            {
                return Results.Ok("user v1");
            }).AllowAnonymous();
            
        }
    }
}
