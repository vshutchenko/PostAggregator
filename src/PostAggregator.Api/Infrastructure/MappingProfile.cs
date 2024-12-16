using AutoMapper;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Dtos.Responses;
using PostAggregator.Api.Services.Models;

namespace PostAggregator.Api.Infrastructure;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(
                src => src.Source.ToString().ToLower()));

        CreateMap<RedditPostData, Post>()
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.CreatedUtc).DateTime))
            .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Url))
            .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => Uri.IsWellFormedUriString(src.Thumbnail, UriKind.Absolute) ? src.Thumbnail : null))
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => Source.Reddit))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text));
    }
}

