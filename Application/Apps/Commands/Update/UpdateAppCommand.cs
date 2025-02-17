using Application.Abstractions.Messaging;

namespace Application.Apps.Commands.Update
{
    public record UpdateAppCommand(
        int Id, 
        string Name, 
        string? Description, 
        string? BaseUrl,
        string ModifiedBy) : ICommand;
}
