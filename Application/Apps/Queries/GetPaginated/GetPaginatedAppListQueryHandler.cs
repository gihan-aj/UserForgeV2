using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Shared.Pagination;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Apps.Queries.GetPaginated
{
    internal sealed class GetPaginatedAppListQueryHandler : IQueryHandler<GetPaginatedAppListQuery, PaginatedList<PaginatedAppResponse>>
    {
        private readonly IAppsRepository _appsRepository;

        public GetPaginatedAppListQueryHandler(IAppsRepository appsRepository)
        {
            _appsRepository = appsRepository;
        }

        public async Task<Result<PaginatedList<PaginatedAppResponse>>> Handle(GetPaginatedAppListQuery request, CancellationToken cancellationToken)
        {
            return await _appsRepository.GetPaginatedAppListAsync(
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize, 
                cancellationToken);
        }
    }
}
