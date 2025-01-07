using Application.Shared.Requesets;

namespace Application.UserManagement.Commands.Delete
{
    public record DeleteUsersRequest(BulkIdsRequest<string> UserIds);
}
