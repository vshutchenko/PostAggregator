using FluentValidation;
using PostAggregator.Api.Dtos.Requests;
using System.Text.RegularExpressions;

namespace PostAggregator.Api.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(post => post.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(post => post.Author).NotEmpty().WithMessage("Author is required.");
        RuleFor(post => post.Text).NotEmpty().WithMessage("Text is required.");
        RuleFor(post => post.Thumbnail)
            .Must(BeNullOrBase64String)
            .WithMessage("Thumbnail must be either null or a valid Base64 string.");
    }

    private bool BeNullOrBase64String(string? value)
    {
        if (value == null) return true;

        var base64ImageRegex = new Regex(@"^data:image\/jpeg;base64,[A-Za-z0-9\+\/=]+$");

        return base64ImageRegex.IsMatch(value);
    }

}
