using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUser
{
    internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IUserService _userService;

        public GetUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<GetUserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var userResult = await _userService.GetByIdAsync(request.UserId);
            if(userResult.IsFailure)
            {
                return Result.Failure<GetUserResponse>(userResult.Error);
            }

            var user = userResult.Value;

            var userDetails = new GetUserResponse(
                user.Id, 
                user.Email!, 
                user.FirstName, 
                user.LastName, 
                user.PhoneNumber, 
                user.DateOfBirth);

            return userDetails;
        }
    }
}
