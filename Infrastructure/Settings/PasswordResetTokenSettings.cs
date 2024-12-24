﻿namespace Infrastructure.Settings
{
    public class PasswordResetTokenSettings
    {
        public string HmacSecretKey { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
