using WebApp.Messages;

namespace WebApp.GraphQL;

public sealed class ItemMutation
{
    public async Task<bool> AddItem([Service]IProducer producer, string name)
    {
        var addItem = new AddItem(name);
        return await producer.ProduceMessageAsync(addItem);
    }

    public async Task<bool> DeleteItem([Service]IProducer producer, int id)
    {
        var deleteItem = new DeleteItem(id);
        return await producer.ProduceMessageAsync(deleteItem);
    }

    public async Task<bool> EditItem([Service]IProducer producer, int id, string name)
    {
        var editItem = new EditItem(id, name);
        return await producer.ProduceMessageAsync(editItem);
    }
}