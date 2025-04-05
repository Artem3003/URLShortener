using AutoMapper;
using URLShortener.Models;
using URLShortener.VM;

namespace URLShortener.Data;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<RegisterViewModel, User>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(vm => vm.Email));
    }
}