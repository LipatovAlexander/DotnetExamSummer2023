using WebApp.Messages;

namespace WebApp.GraphQL;

public sealed class ItemMutation
{
    public async Task<bool> AddItem([Service]IMessageProducer messageProducer, string name)
    {
        var addItem = new AddItem(name);
        return await messageProducer.ProduceAsync(addItem);
    }

    public async Task<bool> DeleteItem([Service]IMessageProducer messageProducer, int id)
    {
        var deleteItem = new DeleteItem(id);
        return await messageProducer.ProduceAsync(deleteItem);
    }

    public async Task<bool> EditItem([Service]IMessageProducer messageProducer, int id, string name)
    {
        var editItem = new EditItem(id, name);
        return await messageProducer.ProduceAsync(editItem);
    }
}