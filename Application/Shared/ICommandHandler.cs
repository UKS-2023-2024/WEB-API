using MediatR;

namespace Application.Shared;

public interface ICommandHandler<TCommand>: IRequestHandler<TCommand> where TCommand:ICommand
{
    
}