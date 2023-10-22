using MediatR;

namespace Application.Shared;

public interface IQuery<out T> : IRequest<T> 
{
}