using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using SharedKernal;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserSettings
{
    internal sealed class GetUserSettingsQueryHandler : IQueryHandler<GetUserSettingsQuery, GetUserSettingsResponse>
    {
        private readonly IUserSettingsRepository _userSettingsRepository;

        public GetUserSettingsQueryHandler(IUserSettingsRepository userSettingsRepository)
        {
            _userSettingsRepository = userSettingsRepository;
        }

        public async Task<Result<GetUserSettingsResponse>> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
        {
            var userSetings = await _userSettingsRepository.GetUserSettings(request.UserId, cancellationToken);

            return new GetUserSettingsResponse(userSetings);
        }
    }
}
