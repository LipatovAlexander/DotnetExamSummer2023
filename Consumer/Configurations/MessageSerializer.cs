using System.Text.Json;
using Confluent.Kafka;
using Consumer.Messages;

namespace Consumer.Configurations;

public sealed class MessageSerializer : ISerializer<IMessage>, IDeserializer<IMessage>
{
    public byte[] Serialize(IMessage data, SerializationContext context)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (data == null)
        {
            return null!;
        }

        var bytes = JsonSerializer.SerializeToUtf8Bytes(data);

        return bytes;
    }

    public IMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null!;
        }

        var json = System.Text.Encoding.UTF8.GetString(data.ToArray());
        var message = JsonSerializer.Deserialize<IMessage>(json);

        return message!;
    }
}