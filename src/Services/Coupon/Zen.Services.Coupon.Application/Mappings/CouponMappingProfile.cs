using AutoMapper;
using Zen.Application.Dtos;
using Zen.Domain.Auditing;
using Zen.Services.Coupon.Application.Dtos;
using Zen.Services.Coupon.Domain.Entities.CouponAggregate;

namespace Zen.Services.Coupon.Application.Mappings;

public class CouponMappingProfile : Profile
{
    public CouponMappingProfile()
    {
        // Mapping from Entity to DTO (Read)
        CreateMap<Domain.Entities.CouponAggregate.Coupon, CouponDto>()
            .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion ?? Array.Empty<byte>())));

        CreateMap<AuditHistoryRecord, AuditHistoryRecordDto>()
                .ForMember(dest => dest.Operation,
                    opt => opt.MapFrom(src => src.Operation.ToString()));
    }
}
