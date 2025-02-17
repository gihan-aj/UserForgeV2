using Application.Abstractions.Messaging;
using Application.Shared.Pagination;

namespace Application.Apps.Queries.GetPaginated
{
    public record GetPaginatedAppListQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IQuery<PaginatedList<PaginatedAppResponse>>;
}
