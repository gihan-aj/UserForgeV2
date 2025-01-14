using System.Linq;
using System.Reflection;

namespace Domain.Permissions
{
    public static class DefaultPermissions
    {
        public static readonly string?[] AllPermissions = typeof(DefaultPermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string?)field.GetValue(null))
            .ToArray();

        public const string UserAccess = "user.access";
        public const string UserRead = "user.read";
        public const string UserStatus = "user.status";
        public const string UserEdit = "user.edit";
        public const string UserDelete = "user.delete";
    }
}
