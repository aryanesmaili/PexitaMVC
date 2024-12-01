using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Core.Entites;

namespace PexitaMVC.Application.MapperConfigs
{
    public class BillMapperConfig : Profile
    {
        public BillMapperConfig()
        {
            CreateMap<BillCreateDTO, BillModel>()
                .ForMember(x => x.IsCompleted, opt => opt.Ignore())
                ;

            CreateMap<BillModel, BillDTO>()
                .ForMember(x => x.Users, opt => opt.MapFrom<BillUserResolver>())
                .ForMember(x => x.Payments, opt => opt.MapFrom<BillPaymentResolver>());

            CreateMap<BillModel, SubBillDTO>();
        }
    }

    public class BillUserResolver(IMapper mapper) : IValueResolver<BillModel, BillDTO, List<SubUserDTO>>
    {
        private readonly IMapper _mapper = mapper;

        public List<SubUserDTO> Resolve(BillModel source, BillDTO destination, List<SubUserDTO> destMember, ResolutionContext context)
        {
            return source.Users.Select(_mapper.Map<SubUserDTO>).ToList();
        }
    }

    public class BillPaymentResolver(IMapper mapper) : IValueResolver<BillModel, BillDTO, List<SubPaymentDTO>>
    {
        private readonly IMapper _mapper = mapper;
        public List<SubPaymentDTO> Resolve(BillModel source, BillDTO destination, List<SubPaymentDTO> destMember, ResolutionContext context)
        {
            return source.BillPayments.Select(_mapper.Map<SubPaymentDTO>).ToList();
        }
    }
}
