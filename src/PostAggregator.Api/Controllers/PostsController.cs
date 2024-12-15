using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PostAggregator.Api.Dtos.Requests;
using PostAggregator.Api.Dtos.Responses;
using PostAggregator.Api.Services.PostService;

namespace PostAggregator.Api.Controllers
{
    /// <summary>
    /// Controller for managing posts.
    /// </summary>
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IOutputCacheStore _cacheStore;

        public PostsController(IPostService postService, IMapper mapper, IOutputCacheStore cacheStore)
        {
            _mapper = mapper;
            _postService = postService;
            _cacheStore = cacheStore;
        }

        /// <summary>
        /// Retrieves a list of posts.
        /// </summary>
        /// <param name="request">Pagination and sorting parameters.</param>
        /// <param name="validator">Validator for checking the request parameters.</param>
        /// <returns>A list of posts.</returns>
        [HttpGet]
        [OutputCache(Tags = ["posts"])]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPostsAsync(
            [FromQuery] PageRequest request,
            [FromServices] IValidator<PageRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var posts = await _postService.GetPostsAsync(request);
            var dtos = posts.Select(_mapper.Map<PostDto>).ToList();

            return Ok(dtos);
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The post's unique identifier.</param>
        /// <returns>The requested post.</returns>
        [HttpGet("{id}")]
        [OutputCache(Tags = ["posts", "{id}"])]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            var postResponse = _mapper.Map<PostDto>(post);

            return Ok(postResponse);
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="request">Data required to create a new post.</param>
        /// <param name="validator">Validator for checking the request data.</param>
        /// <returns>The created post.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePost(
            [FromBody] CreatePostRequest request,
            [FromServices] IValidator<CreatePostRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var createdPost = await _postService.CreatePost(request);
            var postDto = _mapper.Map<PostDto>(createdPost);

            await _cacheStore.EvictByTagAsync("posts", CancellationToken.None);

            return CreatedAtAction(nameof(GetPostById), new { id = postDto.Id }, postDto);
        }
    }
}
