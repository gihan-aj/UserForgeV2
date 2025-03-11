using Domain.Roles;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IRolesRepository
    {
        void Add(Role role);
        Task<bool> RoleNameExists(string roleName, int appId);

        string NormalizeRoleName(string roleName);
    }
}
