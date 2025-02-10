using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Permissions.Queries.GetAll
{
    public record GetAllPermissionsQuery() : IQuery<List<PermissionDetails>>;
}
