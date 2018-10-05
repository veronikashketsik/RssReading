using AutoMapper;
using RssReading.Data.Models;
using RssReading.Domain.Contracts.Models;

namespace RssReading.Data.MapperProfiles
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            this.CreateMap<Source, SourceData>()
                .ReverseMap();
        }
    }
}