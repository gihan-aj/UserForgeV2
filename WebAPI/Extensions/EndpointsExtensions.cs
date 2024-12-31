using Asp.Versioning.Builder;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using WebAPI.Endpoints;

namespace WebAPI.Extensions
{
    public static class EndpointsExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            ApiVersionSet apiVersionSet = app
                .NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();
                
            RouteGroupBuilder versionedGroup = app
                .MapGroup("api/v{apiVersion:apiVersion}")
                .WithApiVersionSet(apiVersionSet);

            versionedGroup.MapUserEndpoints();
            versionedGroup.MapUserManagementEndpoints();
        } 
    }
}
