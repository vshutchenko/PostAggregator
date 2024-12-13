using FluentValidation;
using PostAggregator.Api.Data.Entities;
using PostAggregator.Api.Dtos.Requests;

namespace PostAggregator.Api.Validators;

public class PageRequestValidator : AbstractValidator<PageRequest>
{
    public PageRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.");

        RuleFor(x => x.OrderColumn)
            .Must(BeValidOrderColumn)
            .WithMessage($"OrderColumn must be one of the following: {string.Join(", ", GetPostPropertyNames())}.");
    }

    private bool BeValidOrderColumn(string orderColumn)
    {
        return GetPostPropertyNames().Contains(orderColumn, StringComparer.OrdinalIgnoreCase);
    }

    private static string[] GetPostPropertyNames()
    {
        return typeof(Post).GetProperties().Select(p => p.Name).ToArray();
    }
}
