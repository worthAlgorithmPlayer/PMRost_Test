
namespace PMRost_Test.Application.Contracts.Projects;

public sealed class ProjectFilter
{
    public int Skip { get; set; } = 0;
    public int Limit { get; set; } = 10;
}
