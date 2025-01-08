using System;

namespace Domain.Users
{
    public class UserSettings
    {
        public UserSettings(string userId) 
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Theme = "light";
            DefaultPageSize = 10;
            Language = "en";
            DateFormat = "MM/dd/yyyy";
            TimeFormat = "hh:mm tt";
            TimeZone = "UTC";
            NotificationsEnabled = true;
            EmailNotification = false;
            SmsNotification = false;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string UserId { get; private set; }
        public string Theme {  get; set; }
        public int DefaultPageSize { get; set; }
        public string Language { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string TimeZone { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool EmailNotification { get; set; }
        public bool SmsNotification { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public void Update(
            string theme, 
            string language, 
            string dateFormat, 
            string timeFormat, 
            string timeZone, 
            bool notificationsEnabled, 
            bool emailNotification, 
            bool smsNotification)
        {
            Theme = theme;
            Language = language;
            DateFormat = dateFormat;
            TimeFormat = timeFormat;
            TimeZone = timeZone;
            NotificationsEnabled = notificationsEnabled;
            EmailNotification = emailNotification;
            SmsNotification = smsNotification;
            UpdatedAt = DateTime.UtcNow;
        }

    }

}
