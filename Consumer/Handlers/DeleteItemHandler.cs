using Consumer.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Consumer.Handlers;

public sealed class DeleteItemHandler : IRequestHandler<DeleteItem>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteItemHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteItem request, CancellationToken cancellationToken)
    {
        await _dbContext.Items
            .Where(i => i.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}