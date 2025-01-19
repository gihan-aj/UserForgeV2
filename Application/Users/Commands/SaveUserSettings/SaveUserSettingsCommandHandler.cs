using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.UserSettings;
using SharedKernal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SaveUserSettings
{
    internal sealed class SaveUserSettingsCommandHandler : ICommandHandler<SaveUserSettingsCommand>
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SaveUserSettingsCommandHandler(IUserSettingsRepository userSettingsRepository, IUnitOfWork unitOfWork)
        {
            _userSettingsRepository = userSettingsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SaveUserSettingsCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _userSettingsRepository.GetUserWithSettings(request.UserId, cancellationToken);
            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var user = userResult.Value;

            var settingsToUpdate = new[]
            {
                new UserSetting { Key = "Theme", Value = request.Theme, DataType = "string" }, 
                new UserSetting { Key = "PageSize", Value = request.PageSize.ToString() , DataType = "int"}, 
                new UserSetting { Key = "DateFormat", Value = request.DateFormat, DataType = "string" }, 
                new UserSetting { Key = "TimeFormat", Value = request.TimeFormat, DataType = "string" }
            };

            foreach ( var setting in settingsToUpdate )
            {
                var existingSetting = user.UserSettings.FirstOrDefault(us => us.Key == setting.Key);
                if( existingSetting is not null )
                {
                    existingSetting.Value = setting.Value;
                }
                else
                {
                    setting.UserId = request.UserId;
                    user.UserSettings.Add(setting);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
