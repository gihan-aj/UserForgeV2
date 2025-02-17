using Domain.Apps;
using Domain.Users;

namespace Domain.AppUsers
{
    public class AppUser
    {
        public int AppId { get; set; }
        public App App { get; set; } = null!;
        public string? UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
