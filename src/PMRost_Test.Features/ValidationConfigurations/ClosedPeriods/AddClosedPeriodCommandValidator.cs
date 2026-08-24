
using FluentValidation;
using PMRost_Test.Features.ClosedPeriods;

namespace PMRost_Test.Features.ValidationConfigurations.ClosedPeriods;

public sealed class AddClosedPeriodCommandValidator : AbstractValidator<AddClosedPeriodCommand>
{
    public AddClosedPeriodCommandValidator()
    {
        RuleFor(c => c.Year)
            .InclusiveBetween(2000, 2100)
            .WithMessage("Год должен быть в диапазоне от 2000 до 2100");

        RuleFor(c => c.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Месяц должен быть от 1 до 12");
    }
}
