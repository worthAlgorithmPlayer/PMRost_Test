

using FluentValidation;
using PMRost_Test.Features.TimeEntries;

namespace PMRost_Test.Features.ValidationConfigurations.TimeEntries;

public sealed class CreateTimeEntryCommandValidator : AbstractValidator<CreateTimeEntryCommand>
{
    public CreateTimeEntryCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.TimesheetDate).NotEmpty();
        RuleFor(x => x.Hours).GreaterThan(0).WithMessage("Количество часов должно быть больше 0");
    }
}
