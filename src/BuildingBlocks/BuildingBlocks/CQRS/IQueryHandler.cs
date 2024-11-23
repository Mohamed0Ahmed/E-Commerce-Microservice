using MediatR;

namespace BuildingBlocks.CQRS
{

    public interface IQueryHandler<in TCommand>
       : IRequestHandler<TCommand, Unit>
       where TCommand : ICommand<Unit>
    {
    }

    //*********************

    public interface IQueryHandler<in TQuery, TResponse>
       : IRequestHandler<TQuery, TResponse>
       where TQuery : IQuery<TResponse>
       where TResponse : notnull
    {
    }

}
