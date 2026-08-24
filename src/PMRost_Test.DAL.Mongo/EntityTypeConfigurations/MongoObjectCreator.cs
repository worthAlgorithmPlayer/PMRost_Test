
using System.Runtime.CompilerServices;

namespace PMRost_Test.DAL.Mongo.EntityTypeConfigurations;

internal static class MongoObjectCreator
{
    public static Func<T> UninitializedCreator<T>() =>
        () => (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
}
