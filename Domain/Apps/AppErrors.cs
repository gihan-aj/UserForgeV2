using SharedKernal;

namespace Domain.Apps
{
    public static class AppErrors
    {
        public static class NotFound
        {
            public static Error AppIdNotFound => new("AppIdNotFound", "Cannot find the platform id."); 
        }
    }
}
