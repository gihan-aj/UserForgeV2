namespace Application.Roles.Queries.GetAll
{
    public record GetAllRolesResponse(
        string Id,
        string Name,
        string? Description,
        bool IsActive);
}
