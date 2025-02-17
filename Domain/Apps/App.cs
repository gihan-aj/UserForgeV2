using Domain.Permissions;
using Domain.Primitives;
using Domain.Roles;
using Domain.Users;
using System;
using System.Collections.Generic;

namespace Domain.Apps
{
    public class App : ISoftDeletable, IAuditable
    {
        private App(string name, string? description, string createdBy) 
        {
            Name = name;
            Description = description;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
        
        public string? Description { get; private set; }

        public string? ClientId { get; private set; }

        public string? ClientSecret { get; private set; }

        public string? BaseUrl { get; private set; }

        public bool IsActive { get; private set; } = true;

        public DateTime CreatedOn { get; private set; }

        public string CreatedBy { get; private set; } = null!;

        public DateTime? LastModifiedOn { get; private set; }

        public string? LastModifiedBy { get; private set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<User> Users { get; private set; } = new HashSet<User>();

        public virtual ICollection<Role> Roles { get; private set; } = new HashSet<Role>();

        public virtual ICollection<Permission> Permissions { get; private set; } = new HashSet<Permission>();

        public static App Create(
            string name, 
            string? description, 
            string createdBy)
        {
            return new App(name, description, createdBy);
        }

        public void Update(string name, string? description, string modifiedBy)
        {
            Name = name;
            Description = description;
            LastModifiedBy = modifiedBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        public void Activate(string activatedBy)
        {
            IsActive = true;
            LastModifiedBy = activatedBy;
            LastModifiedOn = DateTime.UtcNow;
        }
        
        public void Deactivate(string deactivatedBy)
        {
            IsActive = false;
            LastModifiedBy = deactivatedBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        //public void AssignRoles(List<Role> roles)
        //{
        //    if(Roles.Count > 0)
        //    {
        //        IEnumerable<Role> rolesRemove = Roles.Except(roles);
        //        if(rolesRemove.Count() > 0)
        //        {
        //            foreach (var role in rolesRemove)
        //            {
        //                Roles.Remove(role);
        //            }
        //        }

        //        IEnumerable<Role> rolesToAdd = roles.Except(Roles);
        //        if(rolesToAdd.Count() > 0)
        //        {
        //            foreach(var role in rolesToAdd)
        //            {
        //                Roles.Add(role);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var role in roles)
        //        {
        //            Roles.Add(role);
        //        }
        //    }
            
        //}

        //public void AssignPermissions(List<Permission> permissions)
        //{
        //    if (Permissions.Count > 0)
        //    {
        //        IEnumerable<Permission> permissionsToRemove = Permissions.Except(permissions);
        //        if (permissionsToRemove.Count() > 0)
        //        {
        //            foreach (var permission in permissionsToRemove)
        //            {
        //                Permissions.Remove(permission);
        //            }
        //        }

        //        IEnumerable<Permission> permissionsToAdd = permissions.Except(Permissions);
        //        if (permissionsToAdd.Count() > 0)
        //        {
        //            foreach (var permission in permissionsToAdd)
        //            {
        //                Permissions.Add(permission);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var permission in permissions)
        //        {
        //            Permissions.Add(permission);
        //        }
        //    }
        //}
    }
}
