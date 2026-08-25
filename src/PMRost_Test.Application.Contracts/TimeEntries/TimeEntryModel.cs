
namespace PMRost_Test.Application.Contracts.TimeEntries;

public sealed class TimeEntryModel
{
    public Guid Id { get; set; }
    public string EmployeeName { get; set; }
    public string ProjectNumber { get; set; }
    public DateOnly TimeSheetDate { get; set; }
    public string? Comment { get; set; }
    public decimal Hours { get; set; }
    public decimal Rate { get; set; }
    public decimal Price { get; set; }
    public int Version { get; set; }
    public bool IsOvertime { get; set; }
}
