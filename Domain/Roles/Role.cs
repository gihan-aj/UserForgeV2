using Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using System;

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

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public void Update(string roleName, string description, string modifiedBy)
        {
            Name = roleName;
            NormalizedName = roleName.ToUpperInvariant();
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
    }
}
