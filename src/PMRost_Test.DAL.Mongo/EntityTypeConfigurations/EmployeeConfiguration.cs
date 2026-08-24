
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PMRost_Test.DAL.Mongo.Serialization;
using PMRost_Test.Domain;

namespace PMRost_Test.DAL.Mongo.EntityTypeConfigurations;

internal sealed class EmployeeConfiguration : MongoCollectionConfiguration<Employee>
{
    protected override string CollectionName => MongoCollectionNames.Employees;

    public override void ConfigureClassMap()
    {
        RegisterHourlyRateClassMap();

        if (BsonClassMap.IsClassMapRegistered(typeof(Employee)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<Employee>(cm =>
        {
            cm.AutoMap();
            cm.SetCreator(MongoObjectCreator.UninitializedCreator<Employee>());
            cm.SetIgnoreExtraElements(true);

            cm.MapProperty(x => x.FullName).SetElementName("fullName").SetIsRequired(true);
            cm.MapProperty(x => x.Department).SetElementName("department").SetIsRequired(true);

            cm.MapField("_hourlyRates").SetElementName("hourlyRates");
            cm.UnmapProperty(x => x.HourlyRates);
        });
    }

    private static void RegisterHourlyRateClassMap()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(EmployeeHourlyRate)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<EmployeeHourlyRate>(cm =>
        {
            cm.AutoMap();
            cm.SetCreator(MongoObjectCreator.UninitializedCreator<EmployeeHourlyRate>());
            cm.SetIgnoreExtraElements(true);

            cm.MapProperty(x => x.EmployeeId).SetElementName("employeeId").SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapProperty(x => x.Rate).SetElementName("rate").SetSerializer(new DecimalSerializer(BsonType.Decimal128));
            cm.MapProperty(x => x.EffectiveFrom).SetElementName("effectiveFrom").SetSerializer(new DateOnlyStringSerializer());
        });
    }

    protected override IEnumerable<CreateIndexModel<Employee>> Indexes()
    {
        yield break;
    }
}
