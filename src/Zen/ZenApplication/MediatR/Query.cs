using Zen.Application.Utilities.Common;

namespace Zen.Application.MediatR.Query;

/// <summary>
/// Marker interface for queries that return a result of type TResult.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IQuery<TResult> { }

/// <summary>
/// Handles queries of type TQuery returning TResult.
/// </summary>
public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<OperationResult<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}