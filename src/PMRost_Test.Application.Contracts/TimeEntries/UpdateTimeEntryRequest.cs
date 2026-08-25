
namespace PMRost_Test.Application.Contracts.TimeEntries;

public sealed class UpdateTimeEntryRequest
{
    public decimal Hours { get; set; }
    public string? Comment { get; set; }
    public int Version { get; set; }
}
