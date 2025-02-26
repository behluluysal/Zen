using AutoMapper;
using Zen.Services.Coupon.Application.Dtos;

namespace Zen.Services.Coupon.Application.Mappings;

public class CouponMappingProfile : Profile
{
    public CouponMappingProfile()
    {
        CreateMap<Domain.Entities.Coupon, CouponDto>();

        CreateMap<CouponDto, Domain.Entities.Coupon>()
            .ConstructUsing(dto => new Domain.Entities.Coupon(dto.Code, dto.Discount, dto.Expiration))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
