
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PMRost_Test.DAL.Mongo.Serialization;
using PMRost_Test.Domain.TimeEntries;

namespace PMRost_Test.DAL.Mongo.EntityTypeConfigurations;

internal sealed class TimeEntryConfiguration : MongoCollectionConfiguration<TimeEntry>
{
    protected override string CollectionName => MongoCollectionNames.TimeEntries;

    public override void ConfigureClassMap()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(TimeEntry)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<TimeEntry>(cm =>
        {
            cm.AutoMap();
            cm.SetCreator(MongoObjectCreator.UninitializedCreator<TimeEntry>());
            cm.SetIgnoreExtraElements(true);

            cm.MapProperty(x => x.EmployeeId).SetElementName("employeeId").SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapProperty(x => x.ProjectId).SetElementName("projectId").SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapProperty(x => x.TimesheetDate).SetElementName("timesheetDate").SetSerializer(new DateOnlyStringSerializer());

            cm.MapProperty(x => x.HalfHours).SetElementName("halfHours");
            cm.UnmapProperty(x => x.Hours);

            cm.MapProperty(x => x.RateApplied).SetElementName("rateApplied").SetSerializer(new DecimalSerializer(BsonType.Decimal128));

            cm.MapProperty(x => x.Comment).SetElementName("comment");
            cm.MapProperty(x => x.IsOvertime).SetElementName("isOvertime");

            cm.MapProperty(x => x.CreatedBy).SetElementName("createdBy");
            cm.MapProperty(x => x.CreatedAtUtc).SetElementName("createdAtUtc");

            cm.MapProperty(x => x.Version).SetElementName("version");
        });
    }

    protected override IEnumerable<CreateIndexModel<TimeEntry>> Indexes()
    {
        var compositeIndexKeys = Builders<TimeEntry>.IndexKeys
            .Ascending(x => x.EmployeeId)
            .Ascending(x => x.TimesheetDate)
            .Ascending(x => x.ProjectId);

        yield return new CreateIndexModel<TimeEntry>(
            compositeIndexKeys,
            new CreateIndexOptions { Name = "ix_timeentry_employee_date_project" });

        var projectIndexKeys = Builders<TimeEntry>.IndexKeys
            .Ascending(x => x.ProjectId)
            .Ascending(x => x.TimesheetDate);

        yield return new CreateIndexModel<TimeEntry>(
            projectIndexKeys,
            new CreateIndexOptions { Name = "ix_timeentry_project_date" });
    }
}