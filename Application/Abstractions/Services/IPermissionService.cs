using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(string userId);
    }
}
