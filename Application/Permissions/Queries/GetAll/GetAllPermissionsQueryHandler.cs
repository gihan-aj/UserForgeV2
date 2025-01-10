using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Shared.Pagination;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permissions.Queries.GetAll
{
    internal sealed class GetAllPermissionsQueryHandler : IQueryHandler<GetAllPermissionsQuery, PaginatedList<GetAllPermissionsResponse>>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public GetAllPermissionsQueryHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task<Result<PaginatedList<GetAllPermissionsResponse>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _permissionsRepository.GetAllAsync(
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize,
                cancellationToken);
        }
    }
}
