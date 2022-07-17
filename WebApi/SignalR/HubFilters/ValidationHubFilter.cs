using Core.ViewModel.MessageViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.SignalR;
using WebApi.Validators.MessageValidators;

namespace WebApi.SignalR.HubFilters;

public class ValidationHubFilter : IHubFilter
{
    private readonly MessageSendViewModelValidator _messageValidator = new MessageSendViewModelValidator();
    
    public async ValueTask<object> InvokeMethodAsync(
        HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {
        ValidationResult result = new ValidationResult();
        var isValid = false;
        string validationMessage = "";
        
        switch (invocationContext.HubMethodName)
        {
            case "SendPrivateMessage":
                var message = invocationContext.HubMethodArguments[0] as MessageSendViewModel;
                result = _messageValidator.Validate(message);
                (isValid, validationMessage) = (result.IsValid, result.ToString());
                break;
            
            case "ReadMessage":
                var messageId = (int) invocationContext.HubMethodArguments[0];
                if (messageId <= 0)
                    (isValid, validationMessage) = (false, "Message id is not valid");
                break;
        }

        if (!isValid)
            throw new HubException($"Entity is not valid. {validationMessage}");

        return next(invocationContext);
    }
}