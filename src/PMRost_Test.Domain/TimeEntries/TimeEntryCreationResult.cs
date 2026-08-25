
namespace PMRost_Test.Domain.TimeEntries;

public sealed record TimeEntryCreationResult(TimeEntry Entry, IReadOnlyCollection<TimeEntry> EntriesMarkedOvertime);

