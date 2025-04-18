using MediatR;

namespace RenStore.Application.Entities.ShoppingCart.Command.Add;

public class AddToCartCommand : IRequest
{
    public Guid ProductId { get; set; }
    public uint Amount { get; set; }
    public string UserId { get; set; }
}