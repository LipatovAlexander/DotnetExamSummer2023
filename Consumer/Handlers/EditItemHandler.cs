using Consumer.Messages;
using MediatR;

namespace Consumer.Handlers;

public sealed class EditItemHandler : IRequestHandler<EditItem>
{
    public async Task Handle(EditItem request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}