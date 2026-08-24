
namespace PMRost_Test.Application.Contracts;

public sealed class ModelPagedResult<TModel>
{
    public IReadOnlyCollection<TModel> Items { get; set; } = new List<TModel>();
    public long TotalCount { get; set; }
}
