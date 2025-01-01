
using SharedKernal;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IRoleManagementService
    {
        Task<Result<string>> CreateAsync(string roleName);
        Task<Result> UpdateAsync(string roleId, string roleName);
    }
}
