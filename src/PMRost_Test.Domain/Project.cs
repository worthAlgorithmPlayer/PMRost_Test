
using PMRost_Test.Domain.Primitives.EntityTemplates;

namespace PMRost_Test.Domain;

public sealed class Project : DomainEntity
{
    /// <summary>
    /// Шифр (уникальный, например П-001)
    /// </summary>
    public string Number { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public decimal Budget { get; private set; }
    public DateOnly StartDate { get; private set; }
    /// <summary>
    /// Дата окончания, null - проект бессрочный
    /// </summary>
    public DateOnly? EndDate { get; private set; }

    private Project(string number, string name, decimal budget, DateOnly startDate, DateOnly? endDate)
    {
        Number = number;
        Name = name;
        Budget = budget;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Project Create(string number, string name, decimal budget, DateOnly startDate, DateOnly? endDate = null)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Шифр проекта обязателен", nameof(number));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название проекта обязательно", nameof(name));
        }

        if (budget < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(budget), "Бюджет не может быть отрицательным");
        }

        if (endDate is not null && endDate.Value < startDate)
        {
            throw new ArgumentException("Дата окончания не может быть раньше даты начала", nameof(endDate));
        }

        return new Project(number.Trim(), name.Trim(), budget, startDate, endDate);
    }

    public bool IsDateWithinProjectPeriod(DateOnly date) => date >= StartDate && (EndDate is null || date <= EndDate);
}
