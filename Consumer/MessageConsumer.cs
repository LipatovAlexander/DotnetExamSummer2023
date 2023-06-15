using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Consumer.Messages;

namespace Consumer;

public sealed class MessageConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, IMessage> _consumer;
    private readonly IAdminClient _adminClient;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IConsumer<Ignore, IMessage> consumer, IAdminClient adminClient, ILogger<MessageConsumer> logger)
    {
        _consumer = consumer;
        _adminClient = adminClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await CreateTopicAsync(stoppingToken);
    
            _consumer.Subscribe("topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;

                Console.WriteLine(message);
            }
        }
        catch (Exception e)
        {
            _consumer.Close();
            throw;
        }
    }
    
    private async Task CreateTopicAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _adminClient.CreateTopicsAsync(new List<TopicSpecification>
                {
                    new() { Name = "topic" }
                });
                break;
            }
            catch (CreateTopicsException ex)
            {
                if (ex.Results.Any(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
                {
                    break;
                }

                _logger.LogWarning("Try create topic. Retry");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}