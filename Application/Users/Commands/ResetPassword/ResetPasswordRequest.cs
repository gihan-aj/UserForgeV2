namespace Application.Users.Commands.ResetPassword
{
    public record ResetPasswordRequest(string UserId, string Token, string NewPassword);
}
