using AutoMapper;
using RssReading.Domain.Contracts.Models;
using RssReading.Web.Models;

namespace RssReading.Web.MapperProfiles
{
    public class FilterProfile : Profile
    {
        public FilterProfile()
        {
            this.CreateMap<RssFilter, RssFilterDto>()
                .ReverseMap();
        }
    }
}
