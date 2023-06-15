using Confluent.Kafka;
using WebApp.Messages;

namespace WebApp;

public interface IProducer
{
    Task<bool> ProduceMessageAsync<TMessage>(TMessage message) where TMessage : IMessage;
}

public sealed class Producer : IProducer
{
    private readonly IProducer<Null, IMessage> _producer;
    private readonly ILogger<Producer> _logger;

    public Producer(IProducer<Null, IMessage> producer, ILogger<Producer> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task<bool> ProduceMessageAsync<TMessage>(TMessage message) where TMessage : IMessage
    {
        var kafkaMessage = new Message<Null, IMessage> { Value = message };
        try
        {
            await _producer.ProduceAsync("messages", kafkaMessage);
            return true;
        }
        catch (ProduceException<Null, IMessage> e)
        {
            _logger.LogError(e, "Error while producing message");
            return false;
        }
    }
}