using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Domain.Entities;

namespace EcommerceProject.Transversal.Mapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile() 
        {
            CreateMap<Customer,CustomerDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

            //FIELDS WITH DIFFERENT NAME
            //CreateMap<Customer, CustomerDto>().ReverseMap()
            //    .ForMember(destination => destination.CustomerId, source => source.MapFrom(src => src.CustomerId));
        }
    }
}