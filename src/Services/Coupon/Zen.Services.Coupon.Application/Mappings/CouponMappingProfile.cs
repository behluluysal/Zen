using AutoMapper;
using Zen.Application.Dtos;
using Zen.Domain.Auditing;
using Zen.Services.Coupon.Application.Dtos;

namespace Zen.Services.Coupon.Application.Mappings;

public class CouponMappingProfile : Profile
{
    public CouponMappingProfile()
    {
        // Mapping from Entity to DTO (Read)
        CreateMap<Domain.Entities.Coupon, CouponDto>()
            .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion ?? Array.Empty<byte>())));

        CreateMap<AuditHistoryRecord, AuditHistoryRecordDto>()
                .ForMember(dest => dest.Operation,
                    opt => opt.MapFrom(src => src.Operation.ToString()));
    }
}
