using AutoMapper;
using Zen.Application.Common.Dtos;
using Zen.Domain.Auditing;

namespace Zen.Services.Coupon.Application.Coupons;

/// <summary>
/// Data Transfer Object representing a coupon.
/// </summary>
public class CouponGetByIdResponse : IConcurrencyAwareDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public DateTimeOffset Expiration { get; set; }
    public string RowVersion { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.CouponAggregate.Coupon, CouponGetByIdResponse>()
            .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion ?? Array.Empty<byte>())));

            // TODO this is a general used mapping. Move it to base projects
            CreateMap<AuditHistoryRecord, AuditHistoryRecordDto>()
                    .ForMember(dest => dest.Operation,
                        opt => opt.MapFrom(src => src.Operation.ToString()));
        }
    }
}