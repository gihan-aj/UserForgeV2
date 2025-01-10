using Application.Abstractions.Messaging;
using Application.Shared.Pagination;

namespace Application.Permissions.Queries.GetAll
{
    public record GetAllPermissionsQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IQuery<PaginatedList<GetAllPermissionsResponse>>;
}
