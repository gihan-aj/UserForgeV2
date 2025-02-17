using Application.Abstractions.Messaging;

namespace Application.Apps.Commands.Create
{
    public record CreateAppCommand(
        string Name, 
        string? Description, 
        string? BaseUrl, 
        string Createdby) : ICommand<int>;
}
