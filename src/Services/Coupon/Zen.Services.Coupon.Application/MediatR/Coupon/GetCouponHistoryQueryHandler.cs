using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Zen.Application.Dtos;
using Zen.Application.MediatR.Common;
using Zen.Domain.Repositories;

namespace Zen.Services.Coupon.Application.MediatR.Coupon;

internal sealed class GetCouponHistoryQueryHandler(
    IAuditHistoryRepository auditHistoryRepository,
    IMapper mapper,
    ILogger<GetCouponHistoryQueryHandler> logger) : IRequestHandler<GetCouponHistoryQuery, ZenOperationResult<IEnumerable<AuditHistoryRecordDto>>>
{
    public async Task<ZenOperationResult<IEnumerable<AuditHistoryRecordDto>>> Handle(GetCouponHistoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving audit history for coupon id: {CouponId}", request.CouponId);

        var auditRecords = await auditHistoryRepository.GetAuditHistoryAsync(request.CouponId, cancellationToken);
        var dtos = mapper.Map<IEnumerable<AuditHistoryRecordDto>>(auditRecords);

        return ZenOperationResult<IEnumerable<AuditHistoryRecordDto>>.Success(dtos);
    }
}