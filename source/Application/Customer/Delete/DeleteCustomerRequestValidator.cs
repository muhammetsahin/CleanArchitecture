using FluentValidation;

namespace CleanArchitecture.Application;

public sealed class DeleteCustomerRequestValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerRequestValidator()
    {
        RuleFor(request => request.Id).Id();
    }
}
