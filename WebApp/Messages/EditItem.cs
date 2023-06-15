namespace WebApp.Messages;

public sealed record EditItem(int Id, string Name) : IMessage;