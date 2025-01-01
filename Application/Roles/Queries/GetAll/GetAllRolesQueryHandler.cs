using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Shared.Pagination;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Roles.Queries.GetAll
{
    internal sealed class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, PaginatedList<GetAllRolesResponse>>
    {
        private readonly IRoleManagementService _roleManagementService;

        public GetAllRolesQueryHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<PaginatedList<GetAllRolesResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return await _roleManagementService.GetAllRolesAsync(
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize,
                cancellationToken);
        }
    }
}
