using FluentValidation;

namespace CleanArchitecture.Application;

public sealed class AddCustomerRequestValidator : AbstractValidator<AddCustomerRequest>
{
    public AddCustomerRequestValidator()
    {
        RuleFor(request => request.Name).Name();
    }
}
