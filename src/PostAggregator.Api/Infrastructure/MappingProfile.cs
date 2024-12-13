using AutoMapper;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Dtos.Responses;

namespace PostAggregator.Api.Infrastructure;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(
                src => src.Source.ToString().ToLower()));
    }
}

