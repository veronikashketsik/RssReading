using System;
using System.Linq.Expressions;
using AutoMapper;
using RssReading.Data.Models;
using RssReading.Domain.Contracts.Models;
using RssReading.Infrastructure;

namespace RssReading.Data.MapperProfiles
{
    public class RssItemProfile: Profile
    {
        public RssItemProfile()
        {
            this.CreateMap<RssItem, RssItemData>()
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.Source.Id))
                .ReverseMap();

            this.CreateMap<Class1, Class2>()
                .ReverseMap();

            this.CreateMap(typeof(PaginatedResponse<>), typeof(PaginatedResponse<>));
        }
    }

    public class Class2
    {
        public string Name { get; set; }
    }

    public class Class1
    {
        public string Name { get; set; }

    }
}