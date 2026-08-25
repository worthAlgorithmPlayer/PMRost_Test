
namespace PMRost_Test.Application.Contracts.TimeEntries;

public sealed class CreateTimeEntryRequest
{
   public Guid EmployeeId { get; set; }
   public Guid ProjectId { get; set; }
   public DateOnly TimesheetDate { get; set; }
   public decimal Hours { get; set; }
   public string? Comment { get; set; }
}
