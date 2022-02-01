using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using TO_DO.ENTİTY.Models;
using TO_DO.SERVİCE.Dtos;

namespace TO_DO.SERVİCE.Infrastructure.AutoMapperProfiler
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile(IDataProtector provider)
        {
            CreateMap<Todo, TodoDto>();
            CreateMap<TodoDto, Todo>();

            CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => provider.Unprotect(src.PasswordEncode)));
            CreateMap<UserDto, User>()
            .ForMember(dest => dest.PasswordEncode, opt => opt.MapFrom(src => provider.Protect(src.Password)));
        }
    }
}