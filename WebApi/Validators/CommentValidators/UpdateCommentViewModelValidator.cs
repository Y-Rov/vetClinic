using Core.ViewModels.CommentViewModels;
using FluentValidation;

namespace WebApi.Validators.CommentValidators;

public class UpdateCommentViewModelValidator : CommentBaseValidator<UpdateCommentViewModel>
{
    public UpdateCommentViewModelValidator() : base()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0)
            .WithMessage("Invalid comment Id");
    }
}