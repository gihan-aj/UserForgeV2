using Domain.RolePermissions;
using Domain.Roles;
using System;
using System.Collections.Generic;

namespace Domain.Permissions
{
    public class Permission
    {
        private Permission(string name, string description)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public virtual ICollection<RolePermission> RolePermissions { get; private set; } = [];

        public static Permission Create(string name, string description)
        {
             return new Permission(name, description);
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

               role.PermissionChanged(modifiedBy);
            }
        }
    }
}
