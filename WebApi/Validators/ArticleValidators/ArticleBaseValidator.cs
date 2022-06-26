using Core.ViewModels.ArticleViewModels;
using FluentValidation;

namespace WebApi.Validators.ArticleValidators;

public class ArticleBaseValidator<T> : AbstractValidator<T> where T : ArticleViewModelBase
{
    public ArticleBaseValidator()
    {
        RuleFor(art => art.Title)
            .MinimumLength(5)
            .WithMessage("Article title length must be greater than 5")
            .MaximumLength(200)
            .WithMessage("Article title length must be lower than 200");
        
        RuleFor(art => art.Body)
            .MinimumLength(5)
            .WithMessage("Article body length must be greater than 5")
            .MaximumLength(6000)
            .WithMessage("Article body length must be lower than 6000");
    }
}