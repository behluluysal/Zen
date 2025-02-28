using Zen.API.Models;
using Zen.Application.MediatR.Common;

namespace Zen.API.Extensions;

public static class OperationResultExtensions
{
    public static ZenApiResponse<T> ToApiResponse<T>(this ZenOperationResult<T> result)
    {
        return result.IsSuccess
            ? ZenApiResponse<T>.Success(result.Data)
            : ZenApiResponse<T>.Failure(result.ErrorCode, result.ErrorMessage);
    }
}