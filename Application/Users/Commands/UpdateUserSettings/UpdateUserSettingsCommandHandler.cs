using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.UpdateUserSettings
{
    internal sealed class UpdateUserSettingsCommandHandler : ICommandHandler<UpdateUserSettingsCommand>
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserSettingsCommandHandler(IUserSettingsRepository userSettingsRepository, IUnitOfWork unitOfWork)
        {
            _userSettingsRepository = userSettingsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            //var existingSettings = await _userSettingsRepository.GetByUserIdAsync(request.UserId);
            //if(existingSettings is null)
            //{
            //    return UserErrors.NotFound.Resource("User settings");
            //}

            //existingSettings.Update(
            //    request.Theme, 
            //    request.Language, 
            //    request.DateFormat, 
            //    request.TimeFormat, 
            //    request.TimeZone, 
            //    request.NotificationsEnabled, 
            //    request.EmailNotification, 
            //    request.SmsNotification);

            //_userSettingsRepository.Update(existingSettings);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
