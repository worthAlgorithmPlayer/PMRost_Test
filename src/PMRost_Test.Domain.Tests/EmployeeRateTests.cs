

using FluentAssertions;

namespace PMRost_Test.Domain.Tests;

public class EmployeeRateTests
{
    [Fact]
    public void GetRateEffectiveOn_ShouldReturnCorrectRateForDate()
    {
        // Arrange
        var employee = Employee.Create("Иванов И. И.", "Проектный");
        employee.SetRate(500m, new DateOnly(2026, 1, 1));
        employee.SetRate(700m, new DateOnly(2026, 3, 1));

        // Act & Assert
        employee.GetRateEffectiveOn(new DateOnly(2026, 2, 15)).Should().Be(500m);
        employee.GetRateEffectiveOn(new DateOnly(2026, 3, 1)).Should().Be(700m);
        employee.GetRateEffectiveOn(new DateOnly(2026, 3, 10)).Should().Be(700m);
        employee.GetRateEffectiveOn(new DateOnly(2025, 12, 31)).Should().BeNull();
    }
}
