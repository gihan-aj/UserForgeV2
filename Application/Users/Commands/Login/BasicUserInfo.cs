namespace Application.Users.Commands.Login
{
    public record BasicUserInfo(
        string Id, 
        string FirstName, 
        string LastName,
        string[] Roles);
}
