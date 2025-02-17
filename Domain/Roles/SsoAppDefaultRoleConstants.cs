using Domain.Permissions;
using System.Linq;
using System.Reflection;

namespace Domain.Roles
{
    public static class SsoAppDefaultRoleConstants
    {
        public static readonly string?[] AllRoles = typeof(SsoAppDefaultRoleConstants)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string?)field.GetValue(null))
            .ToArray();

        public const string SuperAdmin = "superadmin";
        public const string Admin = "admin";
        public const string Manager = "manager";
        public const string User = "user";
    }
}
