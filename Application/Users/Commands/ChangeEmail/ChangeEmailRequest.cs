namespace Application.Users.Commands.ChangeEmail
{
    public record ChangeEmailRequest(string Token, string NewEmail, string Password);
}
