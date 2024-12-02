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
                .ForMember(x => x.User, opt => opt.MapFrom<BillUserResolver>())
                .ForMember(x => x.Payments, opt => opt.MapFrom<BillPaymentResolver>());

            CreateMap<BillModel, SubBillDTO>();
        }
    }

    public class BillUserResolver(IMapper mapper) : IValueResolver<BillModel, BillDTO, SubUserDTO>
    {
        private readonly IMapper _mapper = mapper;

        public SubUserDTO Resolve(BillModel source, BillDTO destination, SubUserDTO destMember, ResolutionContext context)
        {
            return _mapper.Map<SubUserDTO>(source.User);
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
