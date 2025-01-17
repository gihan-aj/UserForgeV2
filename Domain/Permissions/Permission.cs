using Domain.Primitives;
using Domain.RolePermissions;
using Domain.Roles;
using System;
using System.Collections.Generic;

namespace Domain.Permissions
{
    public class Permission : IAuditable, ISoftDeletable
    {
        private Permission(string name, string? description, string createdBy)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            IsActive = true;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string? Description { get; private set; }

        public bool IsActive { get; private set; }

        public string CreatedBy { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public DateTime? LastModifiedOn { get; private set; }

        public string? LastModifiedBy { get; private set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<RolePermission> RolePermissions { get; private set; } = [];

        public static Permission Create(string name, string? description, string createdBy)
        {
             return new Permission(name, description, createdBy);
        } 

        public void Update(string name, string? description, string modifiedBy)
        {
            Name = name;
            Description = string.IsNullOrWhiteSpace(description) ? null : description;
            LastModifiedBy = modifiedBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        public void Activate(string modifideBy)
        {
            IsActive = true;
            LastModifiedBy = modifideBy;
            LastModifiedOn = DateTime.UtcNow;
        }
        
        public void Deactivate(string modifideBy)
        {
            IsActive = false;
            LastModifiedBy = modifideBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        public void AddRolePermissionsRange(List<Role> roles, string modifiedBy)
        {
            foreach (Role role in roles)
            {
                RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = Id
                });

                LastModifiedBy = modifiedBy;
                LastModifiedOn = DateTime.UtcNow;
            }
        }
    }
}
