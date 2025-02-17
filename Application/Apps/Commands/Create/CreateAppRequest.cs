namespace Application.Apps.Commands.Create
{
    public record CreateAppRequest(string Name, string? Description, string? BaseUrl);
}
