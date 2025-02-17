namespace Application.Apps.Queries.GetPaginated
{
    public record PaginatedAppResponse(
        int Id,
        string Name,
        string? Description,
        string? BaseUrl,
        bool IsActive);
}
