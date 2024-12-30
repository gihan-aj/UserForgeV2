namespace Application.Users.Commands.ChangePassword
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword);
}
