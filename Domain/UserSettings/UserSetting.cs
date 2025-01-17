using Domain.Users;

namespace Domain.UserSettings
{
    public class UserSetting
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? DataType { get; set; }
        public string UserId { get; set; } = null!;
    }
}
