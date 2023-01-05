using FluentValidation;

namespace CleanArchitecture.Application;

public static class Validators
{
    public static IRuleBuilderOptions<T, long> Id<T>(this IRuleBuilder<T, long> builder) => builder.NotEmpty().GreaterThan(0);

    public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> builder) => builder.NotEmpty().MinimumLength(3);
}
