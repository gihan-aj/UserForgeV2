using SharedKernal;

namespace Domain.Apps
{
    public static class AppErrors
    {
        public static class NotFound
        {
            public static Error AppNotFound(int id) => new("AppNotFound", $"An app with the id: {id} is not found.");
            public static Error AppsNotFoundToActivate => new("AppsNotFoundToActivate", "Cannot find any apps to activate.");
            public static Error AppsNotFoundToDeactivate => new("AppsNotFoundToDeactivate", "Cannot find any apps to deactivate.");
            public static Error AppsNotFoundToDelete => new("AppsNotFoundToDelete", "Cannot find any apps to delete.");
            public static Error AppIdNotFound => new("AppIdNotFound", "Cannot find the platform id."); 
        }
        public static class Conflict
        {
            public static Error AppNameAlreadyExists(string appName) => new("AppNameAlreadyExists", $"An app with name: {appName} already exists.");
            public static Error ProtectedApp(string name) => new("ProtectedApp", $"The app : {name} cannot be changed.");
        }
    }
}
