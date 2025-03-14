using Microsoft.AspNetCore.Mvc;
using Zen.API.Models;
using Zen.Application.MediatR.Common;

namespace Zen.API.Extensions;

public static class OperationResultExtensions
{
    /// <summary>
    /// Converts a successful operation result into a CreatedAtAction response,
    /// or returns an error response if the operation failed.
    /// </summary>
    public static ActionResult<ZenApiResponse<T>> ToCreatedAtAction<T>(
        this ZenOperationResult<T> operationResult,
        ControllerBase controller,
        string actionName,
        object routeValues)
    {
        return operationResult.IsSuccess
            ? controller.CreatedAtAction(actionName, routeValues, ZenApiResponse<T>.Success(operationResult.Data))
            : controller.StatusCode((int)operationResult.StatusCode, ZenApiResponse<T>.Failure((int)operationResult.StatusCode, operationResult.ErrorMessage));
    }

    /// <summary>
    /// Converts an operation result into an Ok response (or error response) with the data envelope.
    /// </summary>
    public static ActionResult<ZenApiResponse<T>> ToOk<T>(
        this ZenOperationResult<T> operationResult,
        ControllerBase controller)
    {
        return operationResult.IsSuccess
            ? controller.Ok(ZenApiResponse<T>.Success(operationResult.Data))
            : controller.StatusCode((int)operationResult.StatusCode, ZenApiResponse<T>.Failure((int)operationResult.StatusCode, operationResult.ErrorMessage));
    }

    /// <summary>
    /// Converts an operation result (non-generic) into a NoContent response on success,
    /// or an error response on failure.
    /// </summary>
    public static ActionResult<ZenApiResponse> ToNoContent(
        this ZenOperationResult operationResult,
        ControllerBase controller)
    {
        return operationResult.IsSuccess
            ? controller.NoContent()
            : controller.StatusCode((int)operationResult.StatusCode, ZenApiResponse.Failure((int)operationResult.StatusCode, operationResult.ErrorMessage));
    }

    /// <summary>
    /// Converts a generic operation result into a NoContent response on success,
    /// or an error response on failure.
    /// </summary>
    public static ActionResult<ZenApiResponse> ToNoContent<T>(
        this ZenOperationResult<T> operationResult,
        ControllerBase controller)
    {
        return operationResult.IsSuccess
            ? controller.NoContent()
            : controller.StatusCode((int)operationResult.StatusCode, ZenApiResponse.Failure((int)operationResult.StatusCode, operationResult.ErrorMessage));
    }
}