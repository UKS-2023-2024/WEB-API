using MediatR;

namespace Application.Shared;

public interface IQueryHandler<in TQuery, TResult>: IRequestHandler<TQuery, TResult> where TQuery:IQuery<TResult>
{
    
}