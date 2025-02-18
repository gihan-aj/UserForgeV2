using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserApps
{
    public record UserAppResponse(int Id, string Name, string? Description, string? BaseUrl);
}
