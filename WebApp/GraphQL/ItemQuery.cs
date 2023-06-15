namespace WebApp.GraphQL;

public sealed class ItemQuery
{
    public IEnumerable<ItemDto> GetItems([Service]ApplicationDbContext dbContext)
    {
        return dbContext.Items
            .Select(i => new ItemDto
            {
                Id = i.Id,
                Name = i.Name
            });
    }
}