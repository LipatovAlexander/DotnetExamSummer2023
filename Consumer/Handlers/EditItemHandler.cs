using Consumer.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Consumer.Handlers;

public sealed class EditItemHandler : IRequestHandler<EditItem>
{
    private readonly ApplicationDbContext _dbContext;

    public EditItemHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(EditItem request, CancellationToken cancellationToken)
    {
        await _dbContext.Items
            .Where(i => i.Id == request.Id)
            .ExecuteUpdateAsync(calls => calls.SetProperty(i => i.Name, request.Name), cancellationToken);
    }
}