using Domain.Users;
using SharedKernal;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUsersService
    {
        Task<Result<User>> CreateAsync(string firstName, string lastName, string email, string? phoneNumber, DateTime? dateOfBirth, string password);
    }
}
