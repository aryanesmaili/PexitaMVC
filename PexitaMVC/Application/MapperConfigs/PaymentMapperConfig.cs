using AutoMapper;
using PexitaMVC.Application.DTOs;
using PexitaMVC.Core.Entites;

namespace PexitaMVC.Application.MapperConfigs
{
    public class PaymentMapperConfig : Profile
    {
        public PaymentMapperConfig()
        {
            CreateMap<PaymentModel, PaymentDTO>()
                .ForMember(x => x.User, opt => opt.MapFrom<PaymentUserResolver>())
                .ForMember(x => x.Bill, opt => opt.MapFrom<PaymentBillResolver>());
            ;

            CreateMap<PaymentModel, SubPaymentDTO>();
        }
    }

    public class PaymentUserResolver(IMapper mapper) : IValueResolver<PaymentModel, PaymentDTO, SubUserDTO>
    {
        private readonly IMapper _mapper = mapper;

        public SubUserDTO Resolve(PaymentModel source, PaymentDTO destination, SubUserDTO destMember, ResolutionContext context)
        {
            return _mapper.Map<SubUserDTO>(source.User);
        }
    }

    public class PaymentBillResolver(IMapper mapper) : IValueResolver<PaymentModel, PaymentDTO, SubBillDTO>
    {
        private readonly IMapper _mapper = mapper;
        public SubBillDTO Resolve(PaymentModel source, PaymentDTO destination, SubBillDTO destMember, ResolutionContext context)
        {
            return _mapper.Map<SubBillDTO>(source.Bill);
        }
    }
}
