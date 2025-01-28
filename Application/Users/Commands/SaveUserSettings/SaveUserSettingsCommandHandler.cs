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

            foreach ( var setting in request.UserSettings)
            {
                var existingSetting = user.UserSettings.FirstOrDefault(us => us.Key == setting.Key);
                if( existingSetting is not null )
                {
                    existingSetting.Value = setting.Value;
                    existingSetting.DataType = setting.DataType;
                }
                else
                {
                    var newSetting = new UserSetting
                    {
                        Key = setting.Key,
                        Value = setting.Value,
                        DataType = setting.DataType,
                        UserId = request.UserId
                    };
                    user.UserSettings.Add(newSetting);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
