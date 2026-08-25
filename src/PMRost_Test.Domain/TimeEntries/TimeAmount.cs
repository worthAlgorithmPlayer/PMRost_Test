

namespace PMRost_Test.Domain.TimeEntries;

internal static class TimeAmount
{
    public const short MaxHalfHoursPerEntry = 48; // 24 часа
    public const short MaxHalfHoursPerDay = 48; // 24 часа
    public const short OvertimeThresholdHalfHours = 24; // 12 часов

    public static short ToHalfHours(decimal hours)
    {
        if (hours <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Часы должны быть положительными");
        }

        var halfHours = hours * 2;
        if (halfHours != decimal.Truncate(halfHours))
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Часы должны быть кратны 0.5");
        }

        if (halfHours > MaxHalfHoursPerEntry)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), $"Часы в одной записи не могут превышать {MaxHalfHoursPerEntry / 2m}");
        }

        return (short)halfHours;
    }

    public static decimal ToHours(short halfHours) => halfHours / 2m;
}
