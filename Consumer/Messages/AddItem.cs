namespace Consumer.Messages;

public sealed record AddItem(string Name) : IMessage;