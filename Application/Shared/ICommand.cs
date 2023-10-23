using MediatR;

namespace Application.Shared;

public interface ICommand: IRequest {}
public interface ICommand<out TResult>: IRequest<TResult> {}