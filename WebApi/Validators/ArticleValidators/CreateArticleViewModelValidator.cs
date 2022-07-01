using Core.ViewModels.ArticleViewModels;
using FluentValidation;

namespace WebApi.Validators.ArticleValidators;

public class CreateArticleViewModelValidator : ArticleBaseValidator<CreateArticleViewModel>
{
    public CreateArticleViewModelValidator()
    {
        RuleFor(art => art.AuthorId)
            .GreaterThan(0)
            .WithMessage("Invalid author id");
    }
}