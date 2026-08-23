
using PMRost_Test.Domain.Primitives.EntityTemplates;

namespace PMRost_Test.Domain;

public sealed class Employee : DomainEntity
{
    private readonly List<EmployeeHourlyRate> _hourlyRates = new();
    /// <summary>
    /// ФИО пользователя
    /// </summary>
    public string FullName { get; private set; }
    public string Department { get; private set; }
    public IReadOnlyCollection<EmployeeHourlyRate> HourlyRates => _hourlyRates.AsReadOnly();

    private Employee() { }

    private Employee(string fullName, string department)
    {
        FullName = fullName;
        Department = department;
    }

    public static Employee Create(string fullName, string department)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("ФИО сотрудника обязательно", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(department))
        {
            throw new ArgumentException("Отдел сотрудника обязателен", nameof(department));
        }

        return new Employee(fullName.Trim(), department.Trim());
    }
}
/// <summary>
///  Cписок пар, ставка/дата
/// </summary>
public sealed class EmployeeHourlyRate : DomainEntity
{
    public Guid EmployeeId { get; private set; }
    public decimal Rate { get; private set; }
    public DateOnly EffectiveFrom { get; private set; }

    private EmployeeHourlyRate() { }

    private EmployeeHourlyRate(Guid employeeId, decimal rate, DateOnly effectiveFrom)
    {
        EmployeeId = employeeId;
        Rate = rate;
        EffectiveFrom = effectiveFrom;
    }

    internal static EmployeeHourlyRate Create(Guid employeeId, decimal rate, DateOnly effectiveFrom)
    {
        if (rate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "Ставка должна быть положительной");
        }

        return new EmployeeHourlyRate(employeeId, rate, effectiveFrom);
    }
}
