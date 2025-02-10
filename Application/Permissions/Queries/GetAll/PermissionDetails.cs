namespace Application.Permissions.Queries.GetAll
{
    public record PermissionDetails(string Id, string Name, string Description, string?[] RoleNames);
}
