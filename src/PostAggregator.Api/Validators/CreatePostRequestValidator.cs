using FluentValidation;
using PostAggregator.Api.Dtos.Requests;

namespace PostAggregator.Api.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(post => post.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(post => post.Author).NotEmpty().WithMessage("Author is required.");
        RuleFor(post => post.Text).NotEmpty().WithMessage("Text is required.");
    }
}
