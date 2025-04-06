using MediatR;
using Microsoft.Extensions.Logging;
using ShopDevelop.Application.Entities.ShoppingCart.Query.GetByUserId;
using ShopDevelop.Application.Repository;

namespace ShopDevelop.Persistence.Entities.ShoppingCart.Query.GetByUserId;

public class GetShoppingCartItemsByUserIdQueryHandler 
    : IRequestHandler<GetShoppingCartItemsByUserIdQuery, IList<GetShoppingCartItemsByUserIdVm>>
{
    private readonly ILogger<GetShoppingCartItemsByUserIdQueryHandler> logger;
    private readonly IShoppingCartRepository shoppingCartRepository;

    public GetShoppingCartItemsByUserIdQueryHandler(
        ILogger<GetShoppingCartItemsByUserIdQueryHandler> logger, 
        IShoppingCartRepository shoppingCartRepository)
    {
        this.logger = logger;
        this.shoppingCartRepository = shoppingCartRepository;
    }
    
    public async Task<IList<GetShoppingCartItemsByUserIdVm>> Handle(
        GetShoppingCartItemsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handling {nameof(GetShoppingCartItemsByUserIdQueryHandler)}");
        
        var items = await shoppingCartRepository
            .GetCartByUserIdAsync(request.UserId, cancellationToken);
        
        var result = items.Select(item => 
            new GetShoppingCartItemsByUserIdVm(
                item.Id,
                item.Amount,
                item.Product,
                item.ApplicationUserId))
            .ToList();
        
        logger.LogInformation($"Handled {nameof(GetShoppingCartItemsByUserIdQueryHandler)}");
        
        return result;
    }
}