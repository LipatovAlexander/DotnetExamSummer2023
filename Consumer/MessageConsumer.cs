using Confluent.Kafka;
using Consumer.Messages;
using MediatR;

namespace Consumer;

public sealed class MessageConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, IMessage> _consumer;
    private readonly ILogger<MessageConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MessageConsumer(IConsumer<Ignore, IMessage> consumer, ILogger<MessageConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _consumer = consumer;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
}