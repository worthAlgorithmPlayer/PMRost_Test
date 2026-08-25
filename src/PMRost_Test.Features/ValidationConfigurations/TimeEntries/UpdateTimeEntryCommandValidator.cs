
using FluentValidation;
using PMRost_Test.Features.TimeEntries;

namespace PMRost_Test.Features.ValidationConfigurations.TimeEntries;

public sealed class UpdateTimeEntryCommandValidator : AbstractValidator<UpdateTimeEntryCommand>
{
    public UpdateTimeEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Hours).GreaterThan(0).WithMessage("Количество часов должно быть больше 0");
    }
}
