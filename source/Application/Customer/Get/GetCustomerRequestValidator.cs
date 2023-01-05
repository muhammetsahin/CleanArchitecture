using FluentValidation;

namespace CleanArchitecture.Application;

public sealed class GetCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
{
    public GetCustomerRequestValidator()
    {
        RuleFor(request => request.Id).Id();
    }
}
