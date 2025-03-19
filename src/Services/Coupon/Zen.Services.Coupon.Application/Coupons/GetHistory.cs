using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Zen.Application.Common.Dtos;
using Zen.Application.Common.Interfaces;

namespace Zen.Services.Coupon.Application.Coupons;

public record GetCouponHistoryQuery(string CouponId) : IRequest<Result<IEnumerable<AuditHistoryRecordDto>>>;

internal sealed class GetCouponHistoryQueryHandler(
    IAuditHistoryService auditHistoryService,
    IMapper mapper,
    ILogger<GetCouponHistoryQueryHandler> logger) : IRequestHandler<GetCouponHistoryQuery, Result<IEnumerable<AuditHistoryRecordDto>>>
{
    public async Task<Result<IEnumerable<AuditHistoryRecordDto>>> Handle(GetCouponHistoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving audit history for coupon id: {CouponId}", query.CouponId);

        var auditRecords = await auditHistoryService.GetAuditHistoryAsync(query.CouponId, cancellationToken);
        var dtos = mapper.Map<IEnumerable<AuditHistoryRecordDto>>(auditRecords);

        return Result.Success(dtos);
    }
}