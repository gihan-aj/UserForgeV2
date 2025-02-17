using Domain.Apps;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IAppsRepository
    {
        Task<App?> GetByNameAsync(string name);
    }
}
