
using FluentAssertions;
using PMRost_Test.Domain.TimeEntries;
using PMRost_Test.Domain.TimeEntries.Services;

namespace PMRost_Test.Domain.Tests;

public class TimeEntryServiceTests
{
    private readonly TimeEntryService _service = new();

    [Fact]
    public void Create_WhenTotalHoursPerDayExceeds24_ShouldThrowException()
    {
        // Arrange
        var date = new DateOnly(2026, 3, 6);
        var employee = Employee.Create("Петрова А. С.", "Проектный");
        employee.SetRate(600m, new DateOnly(2026, 1, 1));

        var project = Project.Create("П-001", "Цех", 10000m, new DateOnly(2026, 1, 1));

        var firstResult = _service.Create(
            employee, project, date, hours: 18, comment: null,
            employeeEntriesOnSameDate: Array.Empty<TimeEntry>(),
            isPeriodClosed: false, createdBy: "test");

        var existingEntries = new[] { firstResult.Entry };

        // Act
        Action act = () => _service.Create(
            employee, project, date, hours: 10, comment: null,
            employeeEntriesOnSameDate: existingEntries,
            isPeriodClosed: false, createdBy: "test");

        // Assert
        act.Should().Throw<Exception>()
           .WithMessage("*превысят 24*");
    }

    [Fact]
    public void Create_WhenPeriodIsClosed_ShouldThrowTimesheetPeriodClosedException()
    {
        // Arrange
        var date = new DateOnly(2026, 2, 10);
        var employee = Employee.Create("Иванов И. И.", "Отдел");
        employee.SetRate(500m, new DateOnly(2026, 1, 1));
        var project = Project.Create("П-001", "Цех", 10000m, new DateOnly(2026, 1, 1));

        // Act
        Action act = () => _service.Create(
            employee, project, date, hours: 8, comment: null,
            employeeEntriesOnSameDate: Array.Empty<TimeEntry>(),
            isPeriodClosed: true,
            createdBy: "test");

        // Assert
        act.Should().Throw();
    }

    [Theory]
    [InlineData(2025, 12, 31)]
    [InlineData(2026, 4, 1)]
    public void Create_WhenDateIsOutsideProjectPeriod_ShouldThrowException(int year, int month, int day)
    {
        // Arrange
        var entryDate = new DateOnly(year, month, day);
        var employee = Employee.Create("Иванов И. И.", "Отдел");
        employee.SetRate(500m, new DateOnly(2025, 1, 1));

        var project = Project.Create(
            number: "П-001", name: "Реконструкция", budget: 50000m,
            startDate: new DateOnly(2026, 1, 1),
            endDate: new DateOnly(2026, 3, 31));

        // Act
        Action act = () => _service.Create(
            employee, project, entryDate, hours: 8, comment: null,
            employeeEntriesOnSameDate: Array.Empty<TimeEntry>(),
            isPeriodClosed: false, createdBy: "test");

        // Assert
        act.Should().Throw<Exception>()
           .WithMessage("*должна попадать в период проекта*");
    }

    [Fact]
    public void TotalCost_ShouldCalculateCorrectlyAsDecimal()
    {
        // Arrange
        int hours = 7;
        decimal rate = 750.50m;

        // Act
        decimal totalCost = hours * rate;

        // Assert
        totalCost.Should().Be(5253.50m);
        totalCost.Should().BeOfType(typeof(decimal));
    }
}