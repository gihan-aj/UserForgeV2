using Domain.Permissions;
using Domain.Primitives;
using Domain.RolePermissions;
using Domain.UserRoles;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Roles
{
    public class Role : IdentityRole<string>, IAuditable, ISoftDeletable
    {
        public Role(string name, string? description, string createdBy) : base(name)
        {
            Id = Guid.NewGuid().ToString(); // Explicitly initialize the Id
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description;
            IsActive = true;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }

        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public string CreatedBy { get; private set; }

        public DateTime? LastModifiedOn { get; private set; }

        public string? LastModifiedBy { get; private set; }
        
        public DateTime? PermissionModifiedOn { get; private set; }

        public string? PermissionModifiedBy { get; private set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; private set; } = [];

        public virtual ICollection<RolePermission> RolePermissions { get; private set; } = [];

        public void Update(string roleName, string normalizedRoleName, string description, string modifiedBy)
        {
            Name = roleName;
            NormalizedName = normalizedRoleName;
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description;
            LastModifiedOn = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void Activate(string modifiedBy)
        {
            IsActive = true;
            LastModifiedOn = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            LastModifiedOn = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void AddUserRolesRange(List<User> users, string modifiedBy)
        {
            foreach (var user in users)
            {
                UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = Id
                });

                LastModifiedBy = modifiedBy;
                LastModifiedOn = DateTime.UtcNow;
            }
        }

        public void AddRolePermissionsRange(List<Permission> permissions, string modifiedBy)
        {
            foreach (var permission in permissions)
            {
                RolePermissions.Add(new RolePermission
                {
                    RoleId = Id,
                    PermissionId = permission.Id,
                });
            }

            PermissionModifiedBy = modifiedBy;
            PermissionModifiedOn = DateTime.UtcNow;
        }
    }
}
