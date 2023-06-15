using Consumer.Messages;
using MediatR;

namespace Consumer.Handlers;

public sealed class DeleteItemHandler : IRequestHandler<DeleteItem>
{
    public async Task Handle(DeleteItem request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}