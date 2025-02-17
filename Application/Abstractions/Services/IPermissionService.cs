using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(string userId, int appId);
        Task<HashSet<string>> GetPermissionsAsync(string userId, int appId, CancellationToken cancellationToken);
    }
}
