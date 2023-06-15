namespace Consumer.Messages;

public sealed record DeleteItem(int Id) : IMessage;