using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Apps;
using SharedKernal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserApps
{
    internal sealed class GetUserAppsQueryHandler : IQueryHandler<GetUserAppsQuery, List<UserAppResponse>>
    {
        private readonly IUserService _userService;

        public GetUserAppsQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<List<UserAppResponse>>> Handle(GetUserAppsQuery request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserAppsAsync(request.UserId, cancellationToken);
            if (result.IsFailure)
            {
                return Result.Failure<List<UserAppResponse>>(result.Error);
            }

            return result.Value
                .Where(a => a.Name != AppNames.SsoApp)
                .ToList();
        }
    }
}
