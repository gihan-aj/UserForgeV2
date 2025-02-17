namespace Application.Roles.Commands.Create
{
    public record CreateRoleRequest(string RoleName, string Description, int AppId);
}
