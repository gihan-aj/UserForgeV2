using Domain.Permissions;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IPermissionsRepository
    {
        void Add(Permission permission);
        void Remove(Permission permission);
        void Update(Permission permission);
        Task<bool> ExistsAsync(string permissionName);
    }
}
