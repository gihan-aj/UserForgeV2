using Domain.Apps;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IAppManagementService
    {
        Task<List<App>?> GetAppsToActivateAsync(List<int> ids, CancellationToken cancellationToken);
        Task<List<App>?> GetAppsToDeactivateAsync(List<int> ids, CancellationToken cancellationToken);
    }
}
