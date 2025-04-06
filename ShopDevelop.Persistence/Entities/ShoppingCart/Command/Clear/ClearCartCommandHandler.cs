using MediatR;
using Microsoft.Extensions.Logging;
using ShopDevelop.Application.Entities.ShoppingCart.Command.Clear;
using ShopDevelop.Application.Repository;

namespace ShopDevelop.Persistence.Entities.ShoppingCart.Command.Clear;

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand>
{
    private readonly ILogger logger;
    private readonly IShoppingCartRepository shoppingCartRepository;
    
    public ClearCartCommandHandler(
        ILogger<ClearCartCommandHandler> logger,
        IShoppingCartRepository shoppingCartRepository)
    {
        this.logger = logger;
        this.shoppingCartRepository = shoppingCartRepository;
    }
    
    public async Task Handle(ClearCartCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling {nameof(ClearCartCommandHandler)}");
        
        var items = await shoppingCartRepository
            .GetCartByUserIdAsync(request.UserId, cancellationToken);

        foreach (var item in items)
        {
            await shoppingCartRepository.RemoveFromCartAsync(item.Id, cancellationToken);
        }
        
        logger.LogInformation($"Handled {nameof(ClearCartCommandHandler)}");
    }
}