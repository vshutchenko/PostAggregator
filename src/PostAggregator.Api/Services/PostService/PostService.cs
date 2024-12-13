﻿using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Data.Repositories.PostRepository;
using PostAggregator.Api.Data.Specification;
using PostAggregator.Api.Dtos.Requests;

namespace PostAggregator.Api.Services.PostService;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Post> CreatePost(CreatePostRequest createPostRequest)
    {
        var post = new Post()
        {
            Id = Guid.NewGuid(),
            Title = createPostRequest.Title,
            Text = createPostRequest.Text,
            Author = createPostRequest.Author,
            CreatedAtUtc = DateTime.UtcNow,
            Source = Source.PostAggregator,
        };

        var createdPost = await _postRepository.CreatePostAsync(post);

        return createdPost;
    }

    public async Task<Post> GetPostByIdAsync(Guid id)
    {
        var post = await _postRepository.GetPostByIdAsync(id);
        return post;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(PageRequest pageRequest)
    {
        var specification = new Specification();
        specification.AddSpecification(new LimitOffsetSpecification(pageRequest.Page, pageRequest.PageSize));

        if (pageRequest.OrderColumn != null)
        {
            specification.AddSpecification(new OrderBySpecification(pageRequest.OrderColumn, pageRequest.Asc));
        }

        var postEntities = await _postRepository.GetPostsAsync(specification);

        return postEntities;
    }
}