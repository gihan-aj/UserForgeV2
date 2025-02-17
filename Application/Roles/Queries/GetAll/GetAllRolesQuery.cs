using Application.Abstractions.Messaging;
using Application.Shared.Pagination;

namespace Application.Roles.Queries.GetAll
{
    public record GetAllRolesQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize,
        int appId) : IQuery<PaginatedList<GetAllRolesResponse>>;
}
