using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Domain.Entity;

namespace EcommerceProject.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile() 
        {
            CreateMap<Customer,CustomerDto>().ReverseMap();

            //FIELDS WITH DIFFERENT NAME
            //CreateMap<Customer, CustomerDto>().ReverseMap()
            //    .ForMember(destination => destination.CustomerId, source => source.MapFrom(src => src.CustomerId));
        }
    }
}