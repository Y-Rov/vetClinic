using Core.ViewModels.CommentViewModels;
using FluentValidation;

namespace WebApi.Validators.CommentValidators;

public class CommentBaseValidator<T> : AbstractValidator<T> where T : CommentViewModelBase
{
    public CommentBaseValidator()
    {
        RuleFor(c => c.Content)
            .MinimumLength(1)
            .WithMessage("Comment length must be greater than 1")
            .MaximumLength(1000)
            .WithMessage("Comment length must be lower than 1000");
    }
}