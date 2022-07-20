using Core.ViewModel.MessageViewModels;
using FluentValidation;

namespace WebApi.Validators.MessageValidators;

public class MessageSendViewModelValidator : AbstractValidator<MessageSendViewModel>
{
    public MessageSendViewModelValidator()
    {
        RuleFor(m => m.ReceiverId).GreaterThan(0).WithMessage("Invalid Id");
        RuleFor(m => m.Text).NotNull().NotEmpty().Length(1, 600)
            .WithMessage("Message length must be between 1 and 600 characters");
    }
}