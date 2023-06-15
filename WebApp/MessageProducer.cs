using Confluent.Kafka;
using WebApp.Messages;

namespace WebApp;

public interface IMessageProducer
{
    Task<bool> ProduceAsync<TMessage>(TMessage message) where TMessage : IMessage;
}

public sealed class MessageProducer : IMessageProducer
{
    private readonly IProducer<Null, IMessage> _producer;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(IProducer<Null, IMessage> producer, ILogger<MessageProducer> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task<bool> ProduceAsync<TMessage>(TMessage message) where TMessage : IMessage
    {
        var kafkaMessage = new Message<Null, IMessage> { Value = message };
        try
        {
            await _producer.ProduceAsync("topic", kafkaMessage);
            return true;
        }
        catch (ProduceException<Null, IMessage> e)
        {
            _logger.LogError(e, "Error while producing message");
            return false;
        }
    }
}