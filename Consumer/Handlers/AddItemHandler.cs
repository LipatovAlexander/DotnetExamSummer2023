using Consumer.Messages;
using MediatR;

namespace Consumer.Handlers;

public sealed class AddItemHandler : IRequestHandler<AddItem>
{
    public async Task Handle(AddItem request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}