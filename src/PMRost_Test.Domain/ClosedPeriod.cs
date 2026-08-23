
using PMRost_Test.Domain.Primitives.EntityTemplates;

namespace PMRost_Test.Domain;
/// <summary>
/// Закрытый период, блокирует создание, удаление, изменение записей табеля
/// </summary>
public class ClosedPeriod : DomainEntity
{   
    public int Year { get; private set; }
    public int Month { get; private set; }
}
