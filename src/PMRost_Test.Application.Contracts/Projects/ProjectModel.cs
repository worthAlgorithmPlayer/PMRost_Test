
namespace PMRost_Test.Application.Contracts.Projects;

public sealed class ProjectModel
{
    public Guid Id { get; set; }
    public string Number { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Budget { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
