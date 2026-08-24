
using PMRost_Test.Domain.Primitives.EntityTemplates;

namespace PMRost_Test.Domain;
/// <summary>
/// Закрытый период, блокирует создание, удаление, изменение записей табеля
/// </summary>
public class ClosedPeriod : DomainEntity
{   
    public int Year { get; private set; }
    public int Month { get; private set; }

    private ClosedPeriod(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public static ClosedPeriod Create(int year, int month)
    {
        if (year is < 2000 or > 2100)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Некорректный год закрытия периода");
        }

        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Месяц должен быть от 1 до 12");
        }

        return new ClosedPeriod(year, month);
    }
}
