using System.Text.Json.Serialization;

namespace WebApp.Messages;

[JsonDerivedType(typeof(AddItem))]
[JsonDerivedType(typeof(DeleteItem))]
[JsonDerivedType(typeof(EditItem))]
public interface IMessage
{
}