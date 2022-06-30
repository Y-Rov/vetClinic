using Core.ViewModels.ArticleViewModels;
using FluentValidation;

namespace WebApi.Validators.ArticleValidators;

public class UpdateArticleViewModelValidator : ArticleBaseValidator<UpdateArticleViewModel>
{
    public UpdateArticleViewModelValidator() : base()
    {
        RuleFor(art => art.Id)
            .GreaterThan(0)
            .WithMessage("Invalid article Id");
    }
}