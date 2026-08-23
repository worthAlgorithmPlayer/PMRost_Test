
using PMRost_Test.Domain.Primitives.EntityAnnotations;

namespace PMRost_Test.Domain.Primitives.EntityTemplates;

public class DomainEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
