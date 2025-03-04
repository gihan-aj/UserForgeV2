﻿using Application.Abstractions.Messaging;
using Application.Shared.Pagination;

namespace Application.UserManagement.Queries.GetAll
{
    public record GetAllUsersQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IQuery<PaginatedList<GetAllUsersResponse>>;
}
