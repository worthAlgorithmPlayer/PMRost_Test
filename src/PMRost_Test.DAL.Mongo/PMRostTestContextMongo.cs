
using MongoDB.Driver;
using PMRost_Test.Domain;
using PMRost_Test.Domain.TimeEntries;

namespace PMRost_Test.DAL.Mongo;

public static class MongoCollectionNames
{
    public const string Employees = "employees";
    public const string Projects = "projects";
    public const string TimeEntries = "time_entries";
    public const string ClosedPeriods = "closed_periods";
}

public sealed class PMRostTestContextMongo
{
    private readonly IMongoDatabase _db;

    public PMRostTestContextMongo(IMongoDatabase db)
    {
        _db = db;
    }

    public IMongoCollection<Employee> Employees =>
        _db.GetCollection<Employee>(MongoCollectionNames.Employees);
    public IMongoCollection<Project> Projects =>
        _db.GetCollection<Project>(MongoCollectionNames.Projects);
    public IMongoCollection<TimeEntry> TimeEntries =>
        _db.GetCollection<TimeEntry>(MongoCollectionNames.TimeEntries);
    public IMongoCollection<ClosedPeriod> ClosedPeriods =>
        _db.GetCollection<ClosedPeriod>(MongoCollectionNames.ClosedPeriods);
}
