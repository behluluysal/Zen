using Microsoft.AspNetCore.Mvc;
using Zen.API.Models;
using Zen.Application.MediatR.Common;

namespace Zen.API.Extensions;

public static class ApiResponseActionResultExtensions
{
    /// <summary>
    /// Returns CreatedAtAction when the operation succeeds, otherwise returns a StatusCode result.
    /// </summary>
    public static ActionResult<ZenApiResponse<T>> ToCreatedAtAction<T>(
        this ZenOperationResult<T> operationResult,
        ControllerBase controller,
        string actionName,
        object routeValues)
    {
        return operationResult.IsSuccess
            ? controller.CreatedAtAction(actionName, routeValues, ZenApiResponse<T>.Success(operationResult.Data))
            : controller.StatusCode(operationResult.ErrorCode, ZenApiResponse<T>.Failure(operationResult.ErrorCode, operationResult.ErrorMessage));
    }

    /// <summary>
    /// Returns Ok when the operation succeeds, otherwise returns a StatusCode result.
    /// </summary>
    public static ActionResult<ZenApiResponse<T>> ToOk<T>(
        this ZenOperationResult<T> operationResult,
        ControllerBase controller)
    {
        return operationResult.IsSuccess
            ? controller.Ok(ZenApiResponse<T>.Success(operationResult.Data))
            : controller.StatusCode(operationResult.ErrorCode, ZenApiResponse<T>.Failure(operationResult.ErrorCode, operationResult.ErrorMessage));
    }

    /// <summary>
    /// Returns NoContent when the operation succeeds, otherwise returns a StatusCode result.
    /// </summary>
    public static ActionResult<ZenApiResponse> ToNoContent(
        this ZenOperationResult operationResult,
        ControllerBase controller)
    {
        return operationResult.IsSuccess
            ? controller.NoContent()
            : controller.StatusCode(operationResult.ErrorCode, ZenApiResponse.Failure(operationResult.ErrorCode, operationResult.ErrorMessage));
    }
}
