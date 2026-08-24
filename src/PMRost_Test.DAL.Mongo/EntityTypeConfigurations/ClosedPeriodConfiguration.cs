
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PMRost_Test.Domain;


namespace PMRost_Test.DAL.Mongo.EntityTypeConfigurations;

internal sealed class ClosedPeriodConfiguration : MongoCollectionConfiguration<ClosedPeriod>
{
    protected override string CollectionName => MongoCollectionNames.ClosedPeriods;

    public override void ConfigureClassMap()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(ClosedPeriod)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<ClosedPeriod>(cm =>
        {
            cm.AutoMap();
            cm.SetCreator(MongoObjectCreator.UninitializedCreator<ClosedPeriod>());
            cm.SetIgnoreExtraElements(true);

            cm.MapProperty(x => x.Year).SetElementName("year").SetIsRequired(true);
            cm.MapProperty(x => x.Month).SetElementName("month").SetIsRequired(true);
        });
    }

    protected override IEnumerable<CreateIndexModel<ClosedPeriod>> Indexes()
    {
        var keys = Builders<ClosedPeriod>.IndexKeys
            .Ascending(x => x.Year)
            .Ascending(x => x.Month);

        yield return new CreateIndexModel<ClosedPeriod>(
            keys,
            new CreateIndexOptions { Unique = true, Name = "ux_closedperiod_year_month" });
    }
}
