using Domain.Users;
using System;

namespace Domain.RefreshTokens
{
    public class RefreshToken
    {
        public RefreshToken(string token, DateTime expiryDate, string userId, string deviceIdentifierHash)
        {
            Id = new Guid();
            Token = token;
            UserId = userId;
            ExpiryDate = expiryDate;
            UserId = userId;
            DeviceIdentifierHash = deviceIdentifierHash;

        }

        public Guid Id { get; private set; }

        public string Token { get; private set; }

        public DateTime ExpiryDate { get; private set; }

        public string UserId { get; private set; } // Foreign key to User

        public string DeviceIdentifierHash { get; private set; }

        public bool IsRevoked { get; private set; } = false;

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

        public virtual User User { get; set; } = null!;

        public void Revoke()
        {
            IsRevoked = true;
        }

        public void Renew(string token, DateTime expiryDate)
        {
            Token = token;
            ExpiryDate = expiryDate;
            if (IsRevoked)
            {
                IsRevoked = false;
            }
        }
    }
}
