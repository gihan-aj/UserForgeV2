using SharedKernal;

namespace Domain.Apps
{
    public static class AppErrors
    {
        public static class NotFound
        {
            public static Error AppIdNotFound => new("AppIdNotFound", "Cannot find the platform id."); 
        }
        public static class Conflict
        {
            public static Error AppNameAlreadyExists(string appName) => new("AppNameAlreadyExists", $"An app with name: {appName} already exists.");
        }
    }
}
