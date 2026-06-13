using BuildingBlocks.Abstractions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    List<string> Categories,
    decimal Price,
    string ImageFile) : ICommand<CreateProductResult>;

public sealed record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(c => c.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(c => c.Categories).NotEmpty().WithMessage("At least one category is required.");
        RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(c => c.ImageFile).NotEmpty().WithMessage("Image file is required.");
        
    }
}
internal sealed class CreateProductCommandHandler(IDocumentSession documentSession, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //data validation 
        //business validation
        //create
        var product = Product.Create(request.Name, request.Description, request.Categories, request.Price, request.ImageFile);
        
        //save
        documentSession.Store(product);
        await documentSession.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}