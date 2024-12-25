using Domain.Users;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using SharedKernal;
using System;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Application.Abstractions.Services;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly TokenSettings _tokenSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings, IOptions<TokenSettings> tokenSettings)
        {
            _smtpSettings = smtpSettings.Value;
            _tokenSettings = tokenSettings.Value;
        }
        public async Task<Result<User>> SendConfirmationEmailAsync(User user, string token)
        {
            try
            {
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var clientUrl = _tokenSettings.JWT.ClientUrl;
                var confirmEmailPath = _smtpSettings.Routes.ConfirmEmailPath;
                var url = $"{clientUrl}/{confirmEmailPath}?token={token}&userId={user.Id}";

                var appName = _smtpSettings.ApplicationName;

                var body = CreateEmailBody(
                    "Email Confirmation",
                    user.FirstName,
                    "Thank you for registering with us! Please confirm your email by clicking the button below:",
                    "Confirm Email",
                    url,
                    appName);

                using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username, appName),
                    Subject = "Confirm your email",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(user.Email!);

                await smtpClient.SendMailAsync(mailMessage);

                return user;
            }
            catch (Exception ex)
            {
                return Result.Failure<User>(new("EmailServerError", ex.Message));
            }
        }

        public async Task<Result<User>> SendEmailChangeEmailAsync(User user, string token, string newEmail)
        {
            try
            {
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var clientUrl = _tokenSettings.JWT.ClientUrl;
                var changeEmailPath = _smtpSettings.Routes.ChangeEmailPath;
                var url = $"{clientUrl}/{changeEmailPath}?token={token}&userId={user.Id}";

                var appName = _smtpSettings.ApplicationName;

                var body = CreateEmailBody(
                    "Change Email Request",
                    user.FirstName,
                    "To change your email, please click the button below:",
                    "Change Email",
                    url,
                    appName);

                using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username, appName),
                    Subject = "Change Email Request",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(newEmail);

                await smtpClient.SendMailAsync(mailMessage);

                return user;
            }
            catch (Exception ex)
            {
                return Result.Failure<User>(new("EmailServerError", ex.Message));
            }
        }

        public async Task<Result<User>> SendPasswordResetEmailAsync(User user, string token)
        {
            try
            {
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var clientUrl = _tokenSettings.JWT.ClientUrl;
                var resetPasswordPath = _smtpSettings.Routes.ResetPasswordPath;
                var url = $"{clientUrl}/{resetPasswordPath}?token={token}&userId={user.Id}";

                var appName = _smtpSettings.ApplicationName;

                var body = CreateEmailBody(
                    "Password Reset Request",
                    user.FirstName,
                    "To reset your password, please click the button below:",
                    "Reset Password",
                    url,
                    appName);

                using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username, appName),
                    Subject = "Password Reset Request",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(user.Email!);

                await smtpClient.SendMailAsync(mailMessage);

                return user;
            }
            catch (Exception ex)
            {
                return Result.Failure<User>(new("EmailServerError", ex.Message));
            }
        }

        private string CapitalizeFirstLetter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return char.ToUpper(name[0]) + name.Substring(1);
        }

        private string CreateEmailBody(string header, string name, string instructions, string action, string url, string appName)
        {
            var firstName = CapitalizeFirstLetter(name);
            int year = DateTime.UtcNow.Year;

            var bodyStyles = "margin: 0; padding: 0;font-family: Arial, sans-serif; background-color: #fcfaed;";
            //var imgStyles = "max-width: 100%; display: block;";
            var container = "border-spacing: 0; width: 100%;max-width: 600px; margin: 0 auto; background: #fcfaed; padding: 20px; border-radius: 15px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);";
            var headerStyles = "text-align: center; background-color: #5c631d; color: #fcfaed; padding: 10px; border-top-left-radius: 15px; border-top-right-radius: 15px;";
            var content = "padding: 20px; font-size: 16px; line-height: 1.5; color: #1b1c15;";
            var cta = "text-align: center; margin: 20px 0;";
            var buttonStyles = "display: inline-block; background-color: #5c631d; color: #fcfaed; text-decoration: none; padding: 10px 20px; border-radius: 8px; font-size: 16px;";
            var footerStyles = "text-align: center; font-size: 12px; color: #78786a; margin-top: 20px;";
            var footerLinkStyles = "color: #5c631d; text-decoration: none;";

            var mailBody =
                $"<div style=\"{bodyStyles}\">" +
                $"<table role=\"presentation\" style=\"{container}\">" +
                "<tr>" +
                "<td>" +
                $"<div style=\"{headerStyles}\">" +
                $"<h1>{header}</h1>" +
                "</div>" +
                $"<div style=\"{content}\">" +
                $"<p>Dear {firstName},</p>" +
                "<p>" +
                $"{instructions}" +
                "</p>" +
                $"<div style=\"{cta}\">" +
                $"<a style=\"{buttonStyles}\" href=\"{url}\" target=\"_blank\">" +
                $"<strong>{action}</strong></a>" +
                "</div>" +
                "<p>If you didn't request this, you can safely ignore this email.</p>" +
                $"<p>Best regards,<br /> {appName} Team</p>" +
                "</div>" +
                $"<div style=\"{footerStyles}\">" +
                "<p>" +
                "You received this email because you registered at " +
                $"<a style=\"{footerLinkStyles}\" href=\"userforge.com\">{appName}</a>." +
                "</p>" +
                "<p>" +
                "Contact us at" +
                $"<a style=\"{footerLinkStyles}\" href=\"mailto:support@userforge.com\">support@userforge.com</a>" +
                "</p>" +
                "</div>" +
                "</td>" +
                "</tr>" +
                "</table>"+
                "</div>";

            return mailBody;
        }
    }
}
