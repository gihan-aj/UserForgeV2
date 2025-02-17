namespace Application.Apps.Commands.Update
{
    public record UpdateAppRequest(int Id, string Name, string? Description, string? BaseUrl);
}
