using BuildingBlocks.Abstractions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public sealed record DeleteProductResult(bool Success);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID must not be empty.");
    }
}

public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product is null)
        {
            return new DeleteProductResult(false);
        }

        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}