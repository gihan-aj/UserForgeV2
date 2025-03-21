# UserForge

UserForge is a robust and flexible user management system designed to simplify authentication, authorization, and user account management. Built with a clean architecture and utilizing modern development practices, UserForge provides a foundation for scalable, secure, and maintainable applications.

## Key Features

### 1. **Role-Based Access Control (RBAC)**
- Fine-grained permission management based on roles.

### 2. **Single Sign-On (SSO)**
- Serve as a centralized authentication platform for multiple software packages (e.g., POS, inventory management, accounting).
- Secure and seamless login experience across integrated applications.

### 3. **Integration with External Software Packages**
- Easily connect and manage users for third-party applications.
- Provides APIs for user management and authentication integration.

### 4. **Clean Architecture**
- Separation of concerns with clearly defined layers:
  - **Domain Layer**: Core business logic and entities (e.g., Users, Roles, Permissions).
  - **Application Layer**: Use cases and application-specific business rules.
  - **Infrastructure Layer**: Implementation details like database access, email services, etc.
  - **API Layer**: Entry point for client applications.

### 5. **Result Pattern**
- Consistent result handling across all use cases.
- Provides clear feedback for success, errors, and validation issues.

### 6. **Security Features**
- Support for refresh tokens stored securely in the database.
- Device tracking for enhanced account security.
- Comprehensive logging and auditing.

### 7. **User Management**
- Features include registration, email confirmation, login, and profile management.
- Admin capabilities for assigning roles and managing accounts.

### 8. **Extendable and Customizable**
- Designed with extensibility in mind to adapt to your unique requirements.
- Modular structure allows for easy addition of new features and integrations.

## Future Improvements
* Add audit logs to track user actions with Domain Event pattern.
* Add cashing for user permissions 

## Code Snippets

### Permission Authorization Handler
```c#
public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        //string? userId = context.User.Claims.FirstOrDefault(
        //    x => x.Type == JwtRegisteredClaimNames.NameId)?.Value;

        string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? appIdString = context.User.FindFirst("appId")?.Value;

        if (userId is null || appIdString is null)
        {
            return;
        }

        if(!int.TryParse(appIdString, out int appId))
        {
            return;
        }

        using IServiceScope scope = _scopeFactory.CreateScope();

        IPermissionService permissionsService = scope.ServiceProvider
            .GetRequiredService<IPermissionService>();

        HashSet<string> permissions =  await permissionsService
            .GetPermissionsAsync(userId, appId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
```
### Soft Delete
```c#
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var softDeleteEntries = ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in softDeleteEntries)
            {
                entry.State = EntityState.Modified;
                entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
                entry.Property(nameof(ISoftDeletable.DeletedOn)).CurrentValue = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
```

## Getting Started

### Prerequisites
- **Development Environment**: .NET 8, EF Core, Angular (for the front end).
- **Database**: SQL Server.
- **Other Tools**: SMTP server for email functionality.

### Installation

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/your-repository/UserForge.git
   cd UserForge
   ```

2. **Set Up the Database:**
   - Update the connection string in `appsettings.json`.
   - Apply migrations to create the necessary tables:
     ```bash
     dotnet ef database update
     ```

3. **Configure Email Service:**
   - Add SMTP server settings in the Infrastructure layer.

4. **Run the Application:**
   ```bash
   dotnet run
   ```

## License
This project is licensed under the MIT License. See the LICENSE file for details.
