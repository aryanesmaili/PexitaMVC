using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Core.Entites;

namespace PexitaMVC.Application.MapperConfigs
{
    public class UserMapperConfig : Profile
    {
        public UserMapperConfig()
        {
            CreateMap<UserModel, UserDTO>()
                .ForMember(x => x.Bills, opt => opt.MapFrom<UserBillResolver>())
                .ForMember(x => x.Payments, opt => opt.MapFrom<UserPaymentResolver>());

            CreateMap<UserModel, SubUserDTO>();
        }
    }

    public class UserBillResolver(IMapper mapper) : IValueResolver<UserModel, UserDTO, List<SubBillDTO>?>
    {
        private readonly IMapper _mapper = mapper;

        public List<SubBillDTO>? Resolve(UserModel source, UserDTO destination, List<SubBillDTO>? destMember, ResolutionContext context)
        {
            return source.Bills.Select(_mapper.Map<SubBillDTO>).ToList();
        }
    }

    public class UserPaymentResolver(IMapper mapper) : IValueResolver<UserModel, UserDTO, List<SubPaymentDTO>?>
    {
        private readonly IMapper _mapper = mapper;

        public List<SubPaymentDTO>? Resolve(UserModel source, UserDTO destination, List<SubPaymentDTO>? destMember, ResolutionContext context)
        {
            return source.UserPayments.Select(_mapper.Map<SubPaymentDTO>).ToList();
        }
    }
}
