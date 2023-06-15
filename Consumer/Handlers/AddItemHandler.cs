using Consumer.Messages;
using Consumer.Models;
using MediatR;

namespace Consumer.Handlers;

public sealed class AddItemHandler : IRequestHandler<AddItem>
{
    private readonly ApplicationDbContext _dbContext;

    public AddItemHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddItem request, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Name = request.Name
        };

        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}