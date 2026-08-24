
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PMRost_Test.DAL.Mongo.Serialization;
using PMRost_Test.Domain;

namespace PMRost_Test.DAL.Mongo.EntityTypeConfigurations;

internal sealed class ProjectConfiguration : MongoCollectionConfiguration<Project>
{
    protected override string CollectionName => MongoCollectionNames.Projects;

    public override void ConfigureClassMap()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Project)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<Project>(cm =>
        {
            cm.AutoMap();
            cm.SetCreator(MongoObjectCreator.UninitializedCreator<Project>());
            cm.SetIgnoreExtraElements(true);

            cm.MapProperty(x => x.Number).SetElementName("number").SetIsRequired(true);
            cm.MapProperty(x => x.Name).SetElementName("name").SetIsRequired(true);
            cm.MapProperty(x => x.Budget).SetElementName("budget").SetSerializer(new DecimalSerializer(BsonType.Decimal128));
            cm.MapProperty(x => x.StartDate).SetElementName("startDate").SetSerializer(new DateOnlyStringSerializer());
            cm.MapProperty(x => x.EndDate).SetElementName("endDate").SetSerializer(new NullableDateOnlyStringSerializer());
        });
    }

    protected override IEnumerable<CreateIndexModel<Project>> Indexes()
    {
        var keys = Builders<Project>.IndexKeys.Ascending(x => x.Number);

        yield return new CreateIndexModel<Project>(
            keys,
            new CreateIndexOptions { Unique = true, Name = "ux_project_number" });
    }
}
