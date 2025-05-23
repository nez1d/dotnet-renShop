using MediatR;

namespace RenStore.Application.Entities.Category.Commands.Delete;

public class DeleteCategoryCommand : IRequest
{
    public int Id { get; set; }
}