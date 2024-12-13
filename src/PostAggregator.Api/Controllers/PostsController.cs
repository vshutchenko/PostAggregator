using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PostAggregator.Api.Dtos.Requests;
using PostAggregator.Api.Dtos.Responses;
using PostAggregator.Api.Services.PostService;

namespace PostAggregator.Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private IMapper _mapper;

        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostsAsync(
            [FromQuery] PageRequest request,
            [FromServices] IValidator<PageRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var posts = await _postService.GetPostsAsync(request);
            var dtos = posts.Select(_mapper.Map<PostDto>).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            var postResponse = _mapper.Map<PostDto>(post);

            return Ok(postResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(
            [FromBody] CreatePostRequest request,
            [FromServices] IValidator<CreatePostRequest> validator)
        {
            validator.ValidateAndThrow(request);

            var createdPost = await _postService.CreatePost(request);
            var postDto = _mapper.Map<PostDto>(createdPost);

            return CreatedAtAction(nameof(GetPostById), new { id = postDto.Id }, postDto);
        }
    }
}
