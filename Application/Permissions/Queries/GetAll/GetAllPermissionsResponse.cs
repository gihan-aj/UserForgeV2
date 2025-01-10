namespace Application.Permissions.Queries.GetAll
{
    public record GetAllPermissionsResponse(
        string Id,
        string Name,
        string? Description,
        bool IsActive);
}
