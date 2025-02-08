using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.BulkAssignRoles
{
    public record BulkAssignRolesRequest(string[] UserIds, string[] RoleNames);
}
