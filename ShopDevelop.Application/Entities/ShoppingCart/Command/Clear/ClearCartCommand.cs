using MediatR;

namespace ShopDevelop.Application.Entities.ShoppingCart.Command.Clear;

public class ClearCartCommand : IRequest
{
    public string UserId { get; set; }
}