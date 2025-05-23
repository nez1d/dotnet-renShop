using MediatR;

namespace RenStore.Application.Entities.Product.Queries.GetSortedByRating;

public class GetSortedProductsByRatingQuery : IRequest<IList<GetSortedProductsByRatingVm>>
{
    public bool Descending { get; set; }
}