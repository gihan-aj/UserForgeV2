using Domain.Primitives;
using Domain.RefreshTokens;
using Domain.UserRoles;
using Domain.UserSettings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Users
{
    public class User : IdentityUser<string>, IAuditable, ISoftDeletable
    {
        private User(string firstName, string lastName, string email) : base() 
        {
            Id = Guid.NewGuid().ToString(); // Explicitly initialize the Id
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            EmailConfirmed = false;
            TwoFactorEnabled = false;
            CreatedOn = DateTime.UtcNow;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public DateOnly? DateOfBirth { get; set; }

        public bool IsActive { get; private set; } = true;

        public DateTime CreatedOn { get; private set; }

        public string CreatedBy { get; private set; } = null!;

        public DateTime? LastModifiedOn { get; private set; }

        public string? LastModifiedBy { get; private set; }
        
        public DateTime? UserRolesModifiedOn { get; private set; }

        public string? UserRolesModifiedBy { get; private set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserSetting> UserSettings { get; set; } = [];

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = [];

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = [];

        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; } = [];

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        public static User Create(string firstName, string lastName, string email)
        {
            var user = new User(firstName, lastName, email);
            user.CreatedBy = user.Id;

            return user;
        }

        public static User Create(string firstName, string lastName, string email, string createdBy)
        {
            var user = new User(firstName, lastName, email);
            user.CreatedBy = createdBy;

            return user;
        }

        public void Update(string firstName, string lastName, DateOnly? dateOfBirth, string? phoneNumber, string modifiedBy)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth.HasValue
                ? dateOfBirth
                : null;
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber)
                ? null
                : phoneNumber;
            LastModifiedBy = modifiedBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        public void Activate(string modifiedBy)
        {
            IsActive = true;
            LastModifiedBy = modifiedBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            LastModifiedBy = modifiedBy;
            LastModifiedOn = DateTime.UtcNow;
        }

        public void UserRolesChanged(string modifiedBy)
        {
            UserRolesModifiedBy = modifiedBy;
            UserRolesModifiedOn = DateTime.UtcNow;
        }
    }
}
