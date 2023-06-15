namespace WebApp;

public interface IProducer
{
    Task<bool> ProduceMessageAsync<TMessage>(TMessage message) where TMessage : class;
}

public sealed class Producer : IProducer
{
    public async Task<bool> ProduceMessageAsync<TMessage>(TMessage message) where TMessage : class
    {
        throw new NotImplementedException();
    }
}