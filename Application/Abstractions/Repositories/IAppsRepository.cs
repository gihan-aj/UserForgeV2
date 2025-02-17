using Domain.Apps;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IAppsRepository
    {
        Task<App?> GetByNameAsync(string name);
        Task<bool> AppNameExists(string name);
        void Add(App app);

    }
}
