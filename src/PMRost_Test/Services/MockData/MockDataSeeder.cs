using MongoDB.Driver;
using PMRost_Test.DAL.Mongo;
using PMRost_Test.Domain;
using PMRost_Test.Domain.TimeEntries;
using PMRost_Test.Domain.TimeEntries.Services;

namespace PMRost_Test.Services.MockData;

public sealed class MockDataSeeder
{
    private const string CreatedBy = "seed";

    private readonly PMRostTestContextMongo _dbContext;
    private readonly ITimeEntryService _timeEntryService;

    public MockDataSeeder(PMRostTestContextMongo dbContext, ITimeEntryService timeEntryService)
    {
        _dbContext = dbContext;
        _timeEntryService = timeEntryService;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var alreadySeeded = await _dbContext.Employees
            .Find(FilterDefinition<Employee>.Empty)
            .AnyAsync(cancellationToken)
            .ConfigureAwait(false);

        if (alreadySeeded)
        {
            return;
        }

        var ivanov = CreateIvanov();
        var petrova = CreatePetrova();
        await _dbContext.Employees.InsertManyAsync(new[] { ivanov, petrova }, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var projectP001 = CreateProjectP001();
        var projectP002 = CreateProjectP002();
        await _dbContext.Projects.InsertManyAsync(new[] { projectP001, projectP002 }, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var timeEntries = CreateTimeEntries(ivanov, petrova, projectP001, projectP002);
        await _dbContext.TimeEntries.InsertManyAsync(timeEntries, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private static Employee CreateIvanov()
    {
        var employee = Employee.Create("Иванов И. И.", "Проектный");
        employee.SetRate(500m, new DateOnly(2026, 1, 1));
        employee.SetRate(600m, new DateOnly(2026, 3, 1));
        return employee;
    }

    private static Employee CreatePetrova()
    {
        var employee = Employee.Create("Петрова А. С.", "Проектный");
        employee.SetRate(700m, new DateOnly(2026, 2, 1));
        return employee;
    }

    private static Project CreateProjectP001()
        => Project.Create(
            number: "П-001",
            name: "Реконструкция цеха",
            budget: 20000m,
            startDate: new DateOnly(2026, 1, 1),
            endDate: new DateOnly(2026, 3, 31));

    private static Project CreateProjectP002()
        => Project.Create(
            number: "П-002",
            name: "Инженерные сети",
            budget: 5000m,
            startDate: new DateOnly(2026, 3, 1));

    private List<TimeEntry> CreateTimeEntries(Employee ivanov, Employee petrova, Project p001, Project p002)
    {
        var entries = new List<TimeEntry>();

        entries.Add(CreateEntry(ivanov, p001, new DateOnly(2026, 2, 20), 8m, entries));
        entries.Add(CreateEntry(ivanov, p001, new DateOnly(2026, 3, 5), 8m, entries));
        entries.Add(CreateEntry(petrova, p001, new DateOnly(2026, 3, 5), 4m, entries));
        entries.Add(CreateEntry(petrova, p002, new DateOnly(2026, 3, 6), 10m, entries));

        return entries;
    }

    private TimeEntry CreateEntry(Employee employee, Project project, DateOnly date, decimal hours, List<TimeEntry> alreadyCreated)
    {
        var sameDayEntries = alreadyCreated
            .Where(e => e.EmployeeId == employee.Id && e.TimesheetDate == date)
            .ToList();

        var result = _timeEntryService.Create(
            employee, project, date, hours,
            comment: null,
            employeeEntriesOnSameDate: sameDayEntries,
            isPeriodClosed: false,
            createdBy: CreatedBy);

        return result.Entry;
    }
}
