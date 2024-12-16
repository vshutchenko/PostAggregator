using FluentValidation;
using PostAggregator.Api.Dtos.Requests;
using System.Buffers.Text;
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

        var base64Pattern = @"^data:image\/(?:jpeg|png|gif|bmp|webp);base64,";

        var match = Regex.Match(value, base64Pattern);

        if (match.Success)
        {
            var base64Data = value.Substring(match.Length);
            Span<byte> buffer = new Span<byte>(new byte[base64Data.Length]);
            return Convert.TryFromBase64String(base64Data, buffer, out _);
        }

        return false;
    }
}

