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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MessageConsumer(IConsumer<Ignore, IMessage> consumer, IAdminClient adminClient, ILogger<MessageConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _consumer = consumer;
        _adminClient = adminClient;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CreateTopicAsync(stoppingToken);

        _consumer.Subscribe("topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            IMessage message;
            
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                message = consumeResult.Message.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message failed during consuming");
                break;
            }

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(message, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message failed during processing");
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