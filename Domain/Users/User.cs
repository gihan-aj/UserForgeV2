using Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using System;

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

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

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

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
