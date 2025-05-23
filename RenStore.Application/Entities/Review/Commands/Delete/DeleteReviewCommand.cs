using MediatR;

namespace RenStore.Application.Entities.Review.Commands.Delete;

public class DeleteReviewCommand : IRequest
{
    public Guid Id { get; set; }
}