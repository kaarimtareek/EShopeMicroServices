using BuildingBlocks.Abstractions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    List<string> Categories,
    decimal Price,
    string ImageFile) : ICommand<UpdateProductResult>;

public sealed record UpdateProductResult(Product Product);

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Product name is required.").Length(2, 150).WithMessage("Product name must be between 1 and 100 characters.");
        RuleFor(c => c.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(c => c.Categories).NotEmpty().WithMessage("At least one category is required.");
        RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(c => c.ImageFile).NotEmpty().WithMessage("Image file is required.");
    }
}
public class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product is null)
        {
            return new UpdateProductResult(null);
        }

        product.Categories = request.Categories;
        product.Description = request.Description;
        product.ImageFile = request.ImageFile;
        product.Name = request.Name;
        product.Price = request.Price;
        product.UpdatedAt = DateTime.UtcNow;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(product);
    }
}