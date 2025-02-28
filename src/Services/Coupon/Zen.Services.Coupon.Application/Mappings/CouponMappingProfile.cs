using AutoMapper;
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

        // Mapping from DTO to Entity (Write)
        CreateMap<CouponDto, Domain.Entities.Coupon>()
            .ConstructUsing(dto => new Domain.Entities.Coupon(dto.Code, dto.Discount, dto.Expiration))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => Convert.FromBase64String(src.RowVersion)));
    }
}
