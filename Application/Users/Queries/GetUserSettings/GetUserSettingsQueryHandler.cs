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
            //var settings = await _userSettingsRepository.GetByUserIdAsync(request.UserId);
            //if(settings is null)
            //{
            //    return Result.Failure<GetUserSettingsResponse>(UserErrors.NotFound.Resource("User setiings"));
            //}

            //return new GetUserSettingsResponse(
            //    settings.Theme,
            //    settings.Language,
            //    settings.DateFormat,
            //    settings.TimeFormat,
            //    settings.TimeZone,
            //    settings.NotificationsEnabled,
            //    settings.EmailNotification,
            //    settings.SmsNotification);

            return new GetUserSettingsResponse("","","","","",true,true,true);
        }
    }
}
