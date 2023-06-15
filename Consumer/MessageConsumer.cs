using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Consumer.Messages;
using MediatR;

namespace Consumer;

public sealed class MessageConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, IMessage> _consumer;
    private readonly IAdminClient _adminClient;
    private readonly ILogger<MessageConsumer> _logger;
    private readonly IMediator _mediator;

    public MessageConsumer(IConsumer<Ignore, IMessage> consumer, IAdminClient adminClient, ILogger<MessageConsumer> logger, IMediator mediator)
    {
        _consumer = consumer;
        _adminClient = adminClient;
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CreateTopicAsync(stoppingToken);

        _consumer.Subscribe("topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;
                
                await _mediator.Send(message, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while consuming message");
            }
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