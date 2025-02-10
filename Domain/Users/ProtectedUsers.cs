using System.Collections.Generic;

namespace Domain.Users
{
    public static class ProtectedUsers
    {
        public static readonly HashSet<string> Emails =
        [
            "admin@userforge.com",
            "manager@userforge.com",
            "user@userforge.com"
        ];
    }
}
