
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace PMRost_Test.DAL.Mongo.Serialization;

public sealed class DateOnlyStringSerializer : SerializerBase<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();
        return DateOnly.ParseExact(value, Format);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        context.Writer.WriteString(value.ToString(Format));
    }
}

public sealed class NullableDateOnlyStringSerializer : SerializerBase<DateOnly?>
{
    private static readonly DateOnlyStringSerializer Inner = new();

    public override DateOnly? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null;
        }

        return Inner.Deserialize(context, args);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly? value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }

        Inner.Serialize(context, args, value.Value);
    }
}
